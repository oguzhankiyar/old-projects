using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using Newtonsoft.Json.Linq;
using iTunes.Models;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using iTunes.Resources;
using System.Windows.Media;

namespace iTunes
{
    public partial class SongDetailsPage : PhoneApplicationPage
    {
        private static bool isPlaying = false;
        DispatcherTimer timer;
        Song currentSong;
        Image image;
        TextBlock text;

        public SongDetailsPage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            PlayerSeek.Width = (Application.Current.RootVisual.RenderSize.Width - 100) * Player.Position.TotalSeconds / Player.NaturalDuration.TimeSpan.TotalSeconds;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("id"))
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

                image = AddToFavorites.Children.First() as Image;
                text = AddToFavorites.Children.Last() as TextBlock;

                int id = Convert.ToInt32(NavigationContext.QueryString["id"]);
                Song song = Database.Songs.SingleOrDefault(x => x.ID == id);
                if (song != null)
                {
                    SongDetails.DataContext = currentSong = song;

                    if (Function.isFavorited(currentSong))
                    {
                        image.Source = new BitmapImage(new Uri("/Assets/favorite_on.png", UriKind.Relative));
                        text.Text = AppResources.InFavoritesLabel;
                    }
                    if (song.PreviewUrl != null)
                    {
                        Player.Source = new Uri(song.PreviewUrl);
                        PlayerSeekBack.Width = Application.Current.RootVisual.RenderSize.Width - 100;
                    }
                    else
                    {
                        PlayButton.Visibility = Visibility.Collapsed;
                        PlayerSeek.Visibility = Visibility.Collapsed;
                        PlayerSeekBack.Visibility = Visibility.Collapsed;
                    }
                }
            }
            GetMoreSongs();
            base.OnNavigatedTo(e);
        }

        private void GetMoreSongs()
        {
            string query = (currentSong.Artist.Name + "-" + currentSong.Name).Replace(" ", "+");
            HttpWebRequest request = HttpWebRequest.Create("http://api.soundcloud.com/tracks.json?client_id=b45b1aa10f1ac2941910a7f0d10f8e28&q=" + query) as HttpWebRequest;
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
                    JArray array = JArray.Parse(json);
                    List<Song> songs = new List<Song>();
                    for (int i=0;i<array.Count;i++)
                    {
                        try
                        {
                            JObject obj = array[i] as JObject;
                            Song song = new Song()
                            {
                                Name = obj["title"].ToString(),
                                PreviewUrl = obj["stream_url"].ToString() + "?client_id=b45b1aa10f1ac2941910a7f0d10f8e28"
                            };
                            songs.Add(song);
                            Database.Songs.Add(song);
                        }
                        catch (Exception) { }
                    }
                    Dispatcher.BeginInvoke(() =>
                    {
                        MoreSongs.ItemsSource = songs.ToList();
                    });
                }
                catch (Exception) { }
            }
        }

        private void GoHomePage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        private void GoSettings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            isPlaying = false;
            PlayButton.Source = new BitmapImage(new Uri("/Assets/transport.play.png", UriKind.Relative));
        }


        private void PlayButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (isPlaying)
            {
                isPlaying = false;
                PlayButton.Source = new BitmapImage(new Uri("/Assets/transport.play.png", UriKind.Relative));
                Player.Pause();
            }
            else
            {
                isPlaying = true;
                PlayButton.Source = new BitmapImage(new Uri("/Assets/transport.pause.png", UriKind.Relative));
                Player.Play();
                timer.Start();
            }
        }
        private void Artist_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ArtistDetailsPage.xaml?id=" + currentSong.Artist.ID, UriKind.Relative));
        }
        private void Album_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AlbumDetailsPage.xaml?id=" + currentSong.Album.ID, UriKind.Relative));
        }

        private void AddToFavorites_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Function.isFavorited(currentSong))
            {
                Database.DeleteFavorite(currentSong);
                image.Source = new BitmapImage(new Uri("/Assets/favorite_off.png", UriKind.Relative));
                text.Text = AppResources.AddToFavoritesLabel;
            }
            else
            {
                Database.AddFavorite(currentSong);
                image.Source = new BitmapImage(new Uri("/Assets/favorite_on.png", UriKind.Relative));
                text.Text = AppResources.InFavoritesLabel;
            }
        }

        private void MoreSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = MoreSongs.SelectedItem as Song;
            if (selected != null)
                Player.Source = new Uri(selected.PreviewUrl);
        }
    }
}