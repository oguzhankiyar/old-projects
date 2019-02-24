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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace iTunesMusic
{
    public sealed partial class AlbumDetailsPage : Page
    {
        private Album currentAlbum;

        public AlbumDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int id = Convert.ToInt32(e.Parameter.ToString());
            GetAlbumDetails(id);
        }

        private async void GetAlbumDetails(int id)
        {
            currentAlbum = await AlbumRequest.GetDetails(id);
            AlbumDetails.DataContext = null;
            AlbumDetails.DataContext = currentAlbum;
            GetAlbumSongs(id);
        }

        private async void GetAlbumSongs(int id)
        {
            currentAlbum.Songs = await AlbumRequest.GetSongs(id);
            AlbumDetails.DataContext = null;
            AlbumDetails.DataContext = currentAlbum;
        }

        private void Songs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Songs.SelectedItem as SongListItem;
            if (selected != null)
                this.Frame.Navigate(typeof(SongDetailsPage), selected.Id);
        }

        private void ArtistLink_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtistDetailsPage), currentAlbum.Artist.Id);
        }
    }
}
