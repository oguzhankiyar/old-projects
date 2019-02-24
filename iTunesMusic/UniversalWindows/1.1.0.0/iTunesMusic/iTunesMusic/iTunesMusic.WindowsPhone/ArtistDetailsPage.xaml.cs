using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using iTunesMusic.Entities;
using iTunesMusic.Entities.EntityListItems;
using iTunesMusic.Requests;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace iTunesMusic
{
    public sealed partial class ArtistDetailsPage : Page
    {
        private Artist currentArtist;

        public ArtistDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int id = Convert.ToInt32(e.Parameter.ToString());
            GetArtistDetails(id);
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void GetArtistDetails(int id)
        {
            currentArtist = await ArtistRequest.GetDetails(id);
            ArtistDetails.DataContext = currentArtist;
            GetArtistAlbums(id);
            GetArtistSongs(id);
        }

        private async void GetArtistAlbums(int id)
        {
            currentArtist.Albums = await ArtistRequest.GetAlbums(id);
            ArtistDetails.DataContext = null;
            ArtistDetails.DataContext = currentArtist;
        }

        private async void GetArtistSongs(int id)
        {
            List<SongListItem> songs = await ArtistRequest.GetSongs(id);
            currentArtist.Songs = songs;
            if (songs.Count() != 0)
            {
                currentArtist.ImageSource = songs.OrderByDescending(x => x.ReleaseDate).First().ImageSource;
                ArtistImage.Source = new BitmapImage(new Uri(currentArtist.ImageSource.Sizex60));
            }
            ArtistDetails.DataContext = null;
            ArtistDetails.DataContext = currentArtist;
        }

        private void Albums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Albums.SelectedItem as AlbumListItem;
            if (selected != null)
                this.Frame.Navigate(typeof(AlbumDetailsPage), selected.Id);
        }

        private void Songs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Songs.SelectedItem as SongListItem;
            if (selected != null)
                this.Frame.Navigate(typeof(SongDetailsPage), selected.Id);
        }
    }
}
