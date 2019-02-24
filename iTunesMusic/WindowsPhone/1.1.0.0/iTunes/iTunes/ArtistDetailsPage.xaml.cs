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
    public partial class ArtistDetailsPage : PhoneApplicationPage
    {
        private Artist currentArtist;
        public ArtistDetailsPage()
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

            ArtistSongs.SelectedItem = null;
            ArtistAlbums.SelectedItem = null;
            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                int id = Convert.ToInt32(NavigationContext.QueryString["id"]);
                currentArtist = Database.Artists.SingleOrDefault(x => x.ID == id);
                if (currentArtist != null)
                {
                    ArtistDetails.DataContext = currentArtist;
                    GetArtistAlbums();
                    GetArtistSongs();
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
        private void GetArtistAlbums()
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = AppResources.GetArtistAlbumsMessage;
            SystemTray.SetProgressIndicator(this, indicator);
            string url = "https://itunes.apple.com/lookup?id=" + currentArtist.ID + "&entity=album";
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.BeginGetResponse(GetAlbums_CallBack, request);
        }
        private void GetAlbums_CallBack(IAsyncResult result)
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

                    List<Album> albums = new List<Album>();
                    for (int i = 1; i < resultCount; i++)
                    {
                        JObject albumObject = resultArray[i] as JObject;
                        Album album = new Album();
                        album.ID = Convert.ToInt32(albumObject["collectionId"].ToString());
                        album.Name = albumObject["collectionName"].ToString();
                        album.Artist = currentArtist;
                        album.Genre = new Genre() { Name = albumObject["primaryGenreName"].ToString() };
                        album.Image = albumObject["artworkUrl60"].ToString();
                        album.ReleaseDate = Convert.ToDateTime(albumObject["releaseDate"].ToString());
                        albums.Add(album);
                        Database.AddAlbum(album);
                    }
                    currentArtist.Albums = albums;
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
                ArtistDetails.DataContext = null;
                ArtistDetails.DataContext = currentArtist;
            });
        }
        private void GetArtistSongs()
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = AppResources.GetArtistSongsMessage;
            SystemTray.SetProgressIndicator(this, indicator);
            string url = "https://itunes.apple.com/lookup?id=" + currentArtist.ID + "&entity=song";
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
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
                ArtistDetails.DataContext = null;
                ArtistDetails.DataContext = currentArtist;
            });
        }

        private void ArtistAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Album album = ArtistAlbums.SelectedItem as Album;
            if (album != null)
                NavigationService.Navigate(new Uri("/AlbumDetailsPage.xaml?id=" + album.ID, UriKind.Relative));
        }
        private void ArtistSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song song = ArtistSongs.SelectedItem as Song;
            if (song != null)
                NavigationService.Navigate(new Uri("/SongDetailsPage.xaml?id=" + song.ID, UriKind.Relative));
        }
    }
}