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
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using iTunes.Models;
using Newtonsoft.Json.Linq;

namespace iTunes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var query = e.Parameter as string;
            tbSearch.Text = query;
            string url = "https://itunes.apple.com/search?entity=song&term=" + query.ToLower().Replace(" ", "+");
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.BeginGetResponse(Search_Callback, request);
        }

        private void Search_Callback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                WebResponse response = request.EndGetResponse(result);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = reader.ReadToEnd();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                List<Song> results = new List<Song>();
                if (array.Count == 0)
                {
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        new MessageDialog("Aramanıza uygun sonuç bulunamadı!").ShowAsync();
                    });
                }
                else
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject songObject = array[i] as JObject;
                        Song song = new Song();
                        song.ID = Convert.ToInt32(songObject["trackId"].ToString());
                        song.Name = songObject["trackName"].ToString();
                        song.Artist = new Artist()
                        {
                            ID = Convert.ToInt32(songObject["artistId"].ToString()),
                            Name = songObject["artistName"].ToString()
                        };
                        song.Album = new Album()
                        {
                            ID = Convert.ToInt32(songObject["collectionId"].ToString()),
                            Name = songObject["collectionName"].ToString(),
                            Artist = song.Artist
                        };
                        try
                        {
                            song.Genre = new Genre() { Name = songObject["primaryGenreName"].ToString() };
                            song.PreviewUrl = songObject["previewUrl"].ToString();
                        }
                        catch (Exception)
                        {
                        }
                        song.ReleaseDate = Convert.ToDateTime(songObject["releaseDate"].ToString());
                        song.Image = songObject["artworkUrl60"].ToString();
                        results.Add(song);
                        Database.AddSong(song);
                    }
                }
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Results.DataContext = null;
                    Results.DataContext = results;
                });
            }
        }

        private void Search_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && !string.IsNullOrEmpty(tbSearch.Text))
                Frame.Navigate(typeof(SearchPage), tbSearch.Text);
        }

        private void Results_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Song selected = e.ClickedItem as Song;
            if (selected != null)
                Frame.Navigate(typeof(SongDetailsPage), selected.ID);
        }

        private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), tbSearch.Text);
        }
    }
}
