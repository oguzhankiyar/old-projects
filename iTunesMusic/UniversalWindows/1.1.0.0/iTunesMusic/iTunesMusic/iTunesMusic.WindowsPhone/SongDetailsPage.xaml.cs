using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using iTunesMusic.Entities;
using iTunesMusic.Requests;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace iTunesMusic
{
    public sealed partial class SongDetailsPage : Page
    {
        private Song currentSong;

        public SongDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int id = Convert.ToInt32(e.Parameter.ToString());
            GetSongDetails(id);
        }

        private async void GetSongDetails(int id)
        {
            currentSong = await SongRequest.GetDetails(id);
            SongDetails.DataContext = null;
            SongDetails.DataContext = currentSong;
            GetSongSources(currentSong.Artist.Name + " - " + currentSong.Name);
        }

        private async void GetSongSources(string query)
        {
            currentSong.Sources = await SongRequest.GetSources(query);
            SongDetails.DataContext = null;
            SongDetails.DataContext = currentSong;
        }

        private void Sources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Sources.SelectedItem as SongSource;
            if (selected != null)
            {
                Player.Stop();
                Player.Source = new Uri(selected.Source);
                Player.Play();
            }
        }

        private void ArtistLink_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtistDetailsPage), currentSong.Artist.Id);
        }

        private void AlbumLink_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlbumDetailsPage), currentSong.Album.Id);
        }
    }
}
