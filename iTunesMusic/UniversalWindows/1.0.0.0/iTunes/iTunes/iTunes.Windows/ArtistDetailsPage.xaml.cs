using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using iTunes.Models;
using Newtonsoft.Json.Linq;

namespace iTunes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArtistDetailsPage : Page
    {
        private Artist currentArtist;
        public ArtistDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int id = (int)e.Parameter;
            currentArtist = Database.Artists.SingleOrDefault(x => x.ID == id);
            if (currentArtist != null)
            {
                Songs.DataContext = currentArtist;
                string url = "https://itunes.apple.com/lookup?id=" + currentArtist.ID + "&entity=song";
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.BeginGetResponse(GetSongs_CallBack, request);
            }
        }

        private void GetSongs_CallBack(IAsyncResult result)
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

                    List<Song> songs = new List<Song>();
                    for (int i = 1; i < resultCount; i++)
                    {
                        JObject songObject = resultArray[i] as JObject;
                        Song song = new Song();
                        song.ID = Convert.ToInt32(songObject["trackId"].ToString());
                        song.Name = songObject["trackName"].ToString();
                        song.Album = new Album() { ID = Convert.ToInt32(songObject["collectionId"].ToString()), Name = songObject["collectionName"].ToString(), Artist = currentArtist };
                        song.Artist = currentArtist;
                        song.Genre = new Genre() { Name = songObject["primaryGenreName"].ToString() };
                        song.PreviewUrl = songObject["previewUrl"] == null ? null : songObject["previewUrl"].ToString();
                        song.ReleaseDate = Convert.ToDateTime(songObject["releaseDate"].ToString());
                        song.Image = songObject["artworkUrl60"].ToString();
                        songs.Add(song);
                        Database.AddSong(song);
                    }
                    currentArtist.Songs = songs;
                    if (songs.Count() != 0)
                        currentArtist.Image = songs.OrderByDescending(x => x.ReleaseDate).First().Image;
                }
                catch (Exception)
                {
                }
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Songs.DataContext = null;
                Songs.DataContext = currentArtist;
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
