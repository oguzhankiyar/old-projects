using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iTunes.Resources;
using iTunes.Models;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iTunes
{
    public partial class MainPage : PhoneApplicationPage
    {
        ApplicationBarMenuItem MenuItem_Refresh;
        ApplicationBarMenuItem MenuItem_Settings;
        public MainPage()
        {
            InitializeComponent();
        }
        private void CreateApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.MatchOverriddenTheme();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var backStack = NavigationService.BackStack.FirstOrDefault();
            if (backStack != null)
                if (backStack.Source.OriginalString == "/SplashScreenPage.xaml" || backStack.Source.OriginalString == "/InstallationPage.xaml")
                    NavigationService.RemoveBackEntry();

            MenuItem_Refresh = new ApplicationBarMenuItem();
            MenuItem_Refresh.Text = AppResources.RefreshLabel;
            MenuItem_Refresh.Click += new EventHandler(RefreshPage);

            MenuItem_Settings = new ApplicationBarMenuItem();
            MenuItem_Settings.Text = AppResources.SettingsLabel;
            MenuItem_Settings.Click += new EventHandler(GoSettings);
            
            TopSongs.SelectedItem = null;
            TopAlbums.SelectedItem = null;
            FavoriteSongs.SelectedItem = null;
            Results.SelectedItem = null;

            ReloadFavorites();
            
            base.OnNavigatedTo(e);
        }

        private void GoSettings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void RefreshPage(object sender, EventArgs e)
        {
            if (Lists.SelectedIndex == 0)
                GetSongs();
            else if (Lists.SelectedIndex == 1)
                GetAlbums();
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
                            song.Image = ((songObject["im:image"])[1])["label"].ToString();
                        }
                        catch { }
                        Database.AddTopSong(song);
                    }
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (SystemTray.ProgressIndicator != null)
                            SystemTray.ProgressIndicator.IsVisible = false;
                        TopSongs.ItemsSource = null;
                        TopSongs.ItemsSource = Database.TopSongs;
                    });
                }
                catch (Exception)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (SystemTray.ProgressIndicator != null)
                            SystemTray.ProgressIndicator.IsVisible = false;
                        MessageBox.Show(AppResources.InternetConnectionError);
                    });
                }
            }
        }
        private void GetAlbums_Callback(IAsyncResult result)
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
                    Database.EmptyTopAlbums();
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject albumObject = array[i] as JObject;
                        Album album = new Album();
                        album.Sort = i + 1;
                        album.ID = Convert.ToInt32(((albumObject["id"])["attributes"])["im:id"].ToString());
                        album.Name = (albumObject["im:name"])["label"].ToString();
                        album.Artist = new Artist() { Name = (albumObject["im:artist"])["label"].ToString() };
                        album.ReleaseDate = Convert.ToDateTime((albumObject["im:releaseDate"])["label"].ToString());
                        album.Image = ((albumObject["im:image"])[1])["label"].ToString();
                        Database.AddTopAlbum(album);
                    }
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (SystemTray.ProgressIndicator != null)
                            SystemTray.ProgressIndicator.IsVisible = false;
                        TopAlbums.ItemsSource = null;
                        TopAlbums.ItemsSource = Database.TopAlbums;
                    });
                }
                catch (Exception)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (SystemTray.ProgressIndicator != null)
                            SystemTray.ProgressIndicator.IsVisible = false;
                        MessageBox.Show(AppResources.InternetConnectionError);
                    });
                }
            }
        }
        private void Search_Callback(IAsyncResult result)
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
                    JArray array = obj["results"] as JArray;
                    List<Song> results = new List<Song>();
                    if (array.Count == 0)
                        Dispatcher.BeginInvoke(() => lblNotFoundMessage.Visibility = Visibility.Visible);
                    else
                    {
                        Dispatcher.BeginInvoke(() => lblNotFoundMessage.Visibility = Visibility.Collapsed);
                        for (int i = 0; i < array.Count; i++)
                        {
                            JObject songObject = array[i] as JObject;
                            Song song = new Song();
                            song.ID = Convert.ToInt32(songObject["trackId"].ToString());
                            song.Name = songObject["trackName"].ToString();
                            song.Artist = new Artist() { ID = Convert.ToInt32(songObject["artistId"].ToString()), Name = songObject["artistName"].ToString() };
                            song.Album = new Album() { ID = Convert.ToInt32(songObject["collectionId"].ToString()), Name = songObject["collectionName"].ToString(), Artist = song.Artist };
                            song.Genre = new Genre() { Name = songObject["primaryGenreName"].ToString() };
                            song.PreviewUrl = songObject["previewUrl"].ToString();
                            song.ReleaseDate = Convert.ToDateTime(songObject["releaseDate"].ToString());
                            song.Image = songObject["artworkUrl60"].ToString();
                            results.Add(song);
                            Database.AddSong(song);
                        }
                    }
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (SystemTray.ProgressIndicator != null)
                            SystemTray.ProgressIndicator.IsVisible = false;
                        Results.ItemsSource = null;
                        Results.ItemsSource = results;
                    });
                }
                catch (Exception)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (SystemTray.ProgressIndicator != null)
                            SystemTray.ProgressIndicator.IsVisible = false;
                        MessageBox.Show(AppResources.InternetConnectionError);
                    });
                }
            }
        }
        
        private void GetSongs()
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = AppResources.GetSongsMessage;
            SystemTray.SetProgressIndicator(this, indicator);
            HttpWebRequest request = HttpWebRequest.Create(Function.GetURL("TopSongs")) as HttpWebRequest;
            request.BeginGetResponse(GetSongs_CallBack, request);
        }
        private void GetAlbums()
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = AppResources.GetAlbumsMessage;
            SystemTray.SetProgressIndicator(this, indicator);
            HttpWebRequest request = HttpWebRequest.Create(Function.GetURL("TopAlbums")) as HttpWebRequest;
            request.BeginGetResponse(GetAlbums_Callback, request);
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string query = txtSearch.Text.ToLower().Replace(" ", "+");
            string url = "https://itunes.apple.com/search?entity=song&term=" + query;
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = AppResources.SearchMessage;
            SystemTray.SetProgressIndicator(this, indicator);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.BeginGetResponse(Search_Callback, request);
        }

        private void Lists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lists.SelectedIndex == 0 && TopSongs.ItemsSource == null) // top songs list
            {
                CreateApplicationBar();
                ApplicationBar.MenuItems.Add(MenuItem_Refresh);
                ApplicationBar.MenuItems.Add(MenuItem_Settings);
                if (Database.TopSongs.Count() == 0)
                    GetSongs();
                else
                    TopSongs.ItemsSource = Database.TopSongs;
            }
            else if (Lists.SelectedIndex == 1 && TopAlbums.ItemsSource == null) // top albums list
            {
                CreateApplicationBar();
                ApplicationBar.MenuItems.Add(MenuItem_Refresh);
                ApplicationBar.MenuItems.Add(MenuItem_Settings);
                if (Database.TopAlbums.Count() == 0)
                    GetAlbums();
                else
                    TopAlbums.ItemsSource = Database.TopAlbums;
            }
            else if (Lists.SelectedIndex == 2) // favorite list
            {
                CreateApplicationBar();
                ApplicationBar.MenuItems.Add(MenuItem_Settings);
            }
            else if (Lists.SelectedIndex == 3) // search area
            {
                CreateApplicationBar();
                ApplicationBar.MenuItems.Add(MenuItem_Settings);
            }
        }

        private void TopSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song song = TopSongs.SelectedItem as Song;
            if (song != null)
                NavigationService.Navigate(new Uri("/SongDetailsPage.xaml?id=" + song.ID, UriKind.Relative));
        }

        private void Results_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song song = Results.SelectedItem as Song;
            if (song != null)
                NavigationService.Navigate(new Uri("/SongDetailsPage.xaml?id=" + song.ID, UriKind.Relative));
        }

        private void FavoriteSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song song = FavoriteSongs.SelectedItem as Song;
            if (song != null)
                NavigationService.Navigate(new Uri("/SongDetailsPage.xaml?id=" + song.ID, UriKind.Relative));
        }

        private void TopAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Album album = TopAlbums.SelectedItem as Album;
            if (album != null)
                NavigationService.Navigate(new Uri("/AlbumDetailsPage.xaml?id=" + album.ID, UriKind.Relative));
        }
        private void DeleteFavorite_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext as Song;
            Database.DeleteFavorite(selected);
            ReloadFavorites();
        }
        private void ReloadFavorites()
        {
            FavoriteSongs.ItemsSource = null;
            FavoriteSongs.ItemsSource = Database.Favorites;
            if (Database.Favorites.Count() == 0)
                lblEmptyMessage.Visibility = Visibility.Visible;
            else
                lblEmptyMessage.Visibility = Visibility.Collapsed;
        }

    }
}