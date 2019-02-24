using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using iTunes.Models;
using Newtonsoft.Json.Linq;


namespace iTunes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumDetailsPage : Page
    {
        private Album currentAlbum;
        public AlbumDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int id = (int)e.Parameter;
            currentAlbum = Database.Albums.SingleOrDefault(x => x.ID == id);
            if (currentAlbum != null)
            {
                Songs.DataContext = currentAlbum;
                string url = "https://itunes.apple.com/lookup?id=" + currentAlbum.ID + "&entity=song";
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.BeginGetResponse(GetCallBack, request);
            }
        }
        private void GetCallBack(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    string json = sr.ReadToEnd();
                    JObject resultObj = JObject.Parse(json);
                    int resultCount = Convert.ToInt32(resultObj["resultCount"].ToString());
                    JArray resultArray = resultObj["results"] as JArray;

                    if (resultCount != 0)
                    {
                        JObject album = resultArray[0] as JObject;
                        currentAlbum.Artist.ID = Convert.ToInt32(album["artistId"].ToString());
                        currentAlbum.Genre = new Genre() { Name = album["primaryGenreName"].ToString() };
                        currentAlbum.Image = album["artworkUrl60"].ToString();
                    }

                    List<Song> songs = new List<Song>();
                    for (int i = 1; i < resultCount; i++)
                    {
                        JObject songObject = resultArray[i] as JObject;
                        Song song = new Song();
                        song.ID = Convert.ToInt32(songObject["trackId"].ToString());
                        song.Name = songObject["trackName"].ToString();
                        song.Album = new Album() { ID = 823593445, Artist = new Artist() { Name = songObject["artistName"].ToString(), Albums = new List<Album>() } };
                        song.Artist = new Artist() { Name = songObject["artistName"].ToString() };
                        song.PreviewUrl = songObject["previewUrl"] == null ? null : songObject["previewUrl"].ToString();
                        song.ReleaseDate = Convert.ToDateTime(songObject["releaseDate"].ToString());
                        song.Image = songObject["artworkUrl60"].ToString();
                        songs.Add(song);
                        Database.AddSong(song);
                    }
                    currentAlbum.Songs = songs;
                }
                catch (Exception)
                {
                }
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Songs.DataContext = null;
                Songs.DataContext = currentAlbum;
            });
        }

        private void Search_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && !string.IsNullOrEmpty(tbSearch.Text))
                Frame.Navigate(typeof(SearchPage), tbSearch.Text);
        }

        private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), tbSearch.Text);
        }

        private void Songs_OnItemClick_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Song selected = e.ClickedItem as Song;
            if (selected != null)
                Frame.Navigate(typeof(SongDetailsPage), selected.ID);
        }
    }
}
