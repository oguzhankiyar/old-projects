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
using iTunes.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Newtonsoft.Json.Linq;

namespace iTunes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            HttpWebRequest request = HttpWebRequest.Create(Function.GetURL("TopSongs")) as HttpWebRequest;
            request.BeginGetResponse(GetSongs_CallBack, request);
        }

        private void GetSongs_CallBack(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string json = reader.ReadToEnd();
                    JObject obj = JObject.Parse(json);
                    JArray array = (obj["feed"])["entry"] as JArray;
                    Database.EmptyTopSongs();
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject songObject = array[i] as JObject;
                        Song song = new Song();
                        song.Sort = i + 1;
                        song.ID = Convert.ToInt32(((songObject["id"])["attributes"])["im:id"].ToString());
                        song.Name = (songObject["im:name"])["label"].ToString();
                        int colID = 0;
                        try
                        {
                            string collectionID = (((songObject["im:collection"])["link"])["attributes"])["href"].ToString();
                            string[] split = collectionID.Split('/');
                            string[] split2 = split[split.Length - 1].Split('?');
                            colID = Convert.ToInt32(split2[0].Replace("id", ""));
                        }
                        catch { }
                        int artID = 0;
                        try
                        {
                            string artistID = ((songObject["im:artist"])["attributes"])["href"].ToString();
                            string[] split = artistID.Split('/');
                            string[] split2 = split[split.Length - 1].Split('?');
                            artID = Convert.ToInt32(split2[0].Replace("id", ""));
                        }
                        catch { }
                        try
                        {
                            song.Genre = new Genre() { ID = Convert.ToInt32(((songObject["category"])["attributes"])["im:id"].ToString()), Name = ((songObject["category"])["attributes"])["label"].ToString() };
                            song.Artist = new Artist() { ID = artID, Name = (songObject["im:artist"])["label"].ToString(), Genre = song.Genre };
                            song.Album = new Album() { ID = colID, Name = ((songObject["im:collection"])["im:name"])["label"].ToString(), Artist = song.Artist };
                            song.PreviewUrl = (((songObject["link"])[1])["attributes"])["href"].ToString();
                            song.ReleaseDate = Convert.ToDateTime((songObject["im:releaseDate"])["label"].ToString());
                            song.Image = ((songObject["im:image"])[2])["label"].ToString();
                        }
                        catch { }
                        Database.AddTopSong(song);
                    }
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TopSongs.DataContext = null;
                        TopSongs.DataContext = Database.TopSongs.ToList();
                    });
                }
                catch (Exception)
                {
                }
            }
        }

        private void Search_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && !string.IsNullOrEmpty(tbSearch.Text))
                Frame.Navigate(typeof(SearchPage), tbSearch.Text);
        }

        private void TopSongs_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Song selected = e.ClickedItem as Song;
            if (selected != null)
                Frame.Navigate(typeof(SongDetailsPage), selected.ID);
        }
    }
}
