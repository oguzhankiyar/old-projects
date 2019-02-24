using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using iTunes.Models;

namespace iTunes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SongDetailsPage : Page
    {
        private static bool isPlaying = false;
        DispatcherTimer timer;
        private Song currentSong;

        public SongDetailsPage()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick +=timer_Tick;
        }

        private void timer_Tick(object sender, object e)
        {
            PlayerSeek.Width = (1200 - 100) * Player.Position.TotalSeconds / Player.NaturalDuration.TimeSpan.TotalSeconds;
        }

        public void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            isPlaying = false;
            PlayButton.Source = new BitmapImage(new Uri("ms-appx:/Assets/transport.pause.png", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int songID = (int)e.Parameter;
            currentSong = Database.Songs.SingleOrDefault(x => x.ID == songID);
            SongDetails.DataContext = currentSong;

            Player.Source = new Uri(currentSong.PreviewUrl);
            PlayerSeekBack.Width = 1200 - 100;
        }

        public void Search_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && !string.IsNullOrEmpty(tbSearch.Text))
                Frame.Navigate(typeof(SearchPage), tbSearch.Text);
        }

        private void PlayButton_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (isPlaying)
            {
                isPlaying = false;
                PlayButton.Source = new BitmapImage(new Uri("ms-appx:/Assets/transport.play.png", UriKind.RelativeOrAbsolute));
                Player.Pause();
            }
            else
            {
                isPlaying = true;
                PlayButton.Source = new BitmapImage(new Uri("ms-appx:/Assets/transport.pause.png", UriKind.RelativeOrAbsolute));
                Player.Play();
                timer.Start();
            }
        }

        private void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), tbSearch.Text);
        }

        private void Album_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AlbumDetailsPage), currentSong.Album.ID);
        }

        private void Artist_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ArtistDetailsPage), currentSong.Artist.ID);
        }
    }
}
