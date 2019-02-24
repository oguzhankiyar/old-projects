using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iTunes.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using iTunes.Resources;

namespace iTunes
{
    public partial class AlbumDetailsPage : PhoneApplicationPage
    {
        private Album currentAlbum;
        public AlbumDetailsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.MatchOverriddenTheme();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBarMenuItem MenuItem_HomePage = new ApplicationBarMenuItem();
            MenuItem_HomePage.Text = AppResources.HomePageLabel;
            MenuItem_HomePage.Click += new EventHandler(GoHomePage);
            ApplicationBarMenuItem MenuItem_Settings = new ApplicationBarMenuItem();
            MenuItem_Settings.Text = AppResources.SettingsLabel;
            MenuItem_Settings.Click += new EventHandler(GoSettings);
            ApplicationBar.MenuItems.Add(MenuItem_HomePage);
            ApplicationBar.MenuItems.Add(MenuItem_Settings);

            AlbumSongs.SelectedItem = null;
            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                int id = Convert.ToInt32(NavigationContext.QueryString["id"]);
                currentAlbum = Database.Albums.SingleOrDefault(x => x.ID == id);
                if (currentAlbum != null)
                {
                    AlbumDetails.DataContext = currentAlbum;
                    GetAlbumSongs();
                }
            }
            base.OnNavigatedTo(e);
        }
        private void GoHomePage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        private void GoSettings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
        private void GetAlbumSongs()
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = AppResources.GetAlbumSongsMessage;
            SystemTray.SetProgressIndicator(this, indicator);
            string url = "https://itunes.apple.com/lookup?id=" + currentAlbum.ID + "&entity=song";
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.BeginGetResponse(GetCallBack, request);
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
                    Dispatcher.BeginInvoke(() =>
                    {
                        SystemTray.ProgressIndicator.IsVisible = false;
                        MessageBox.Show(AppResources.InternetConnectionError);
                    });
                }
            }
            Dispatcher.BeginInvoke(() =>
            {
                SystemTray.ProgressIndicator.IsVisible = false;
                AlbumDetails.DataContext = null;
                AlbumDetails.DataContext = currentAlbum;
            });
        }

        private void AlbumSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song song = AlbumSongs.SelectedItem as Song;
            if (song != null)
                NavigationService.Navigate(new Uri("/SongDetailsPage.xaml?id=" + song.ID, UriKind.Relative));
        }

        private void Artist_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ArtistDetailsPage.xaml?id=" + currentAlbum.Artist.ID, UriKind.Relative));
        }
    }
}