using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using iTunesMusic.Entities;
using iTunesMusic.Entities.EntityListItems;
using iTunesMusic.Requests;
using iTunesMusic.Tools;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace iTunesMusic
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackgroundMediaPlayer.Current.Play();
            GetSongs();
            GetAlbums();
            GetPlaylists();
        }

        private async void GetSongs()
        {
            List<SongListItem> songs = await SongRequest.GetTopSongs();
            TopSongs.DataContext = null;
            TopSongs.DataContext = songs;
        }

        private async void GetAlbums()
        {
            List<AlbumListItem> albums = await AlbumRequest.GetTopAlbums();
            TopAlbums.DataContext = null;
            TopAlbums.DataContext = albums;
        }

        private async void GetPlaylists()
        {
            Database.Playlists.Add(new Playlist() { Name = "Lana Del Rey", Songs = new List<SongListItem>() });
            Playlists.DataContext = null;
            Playlists.DataContext = Database.Playlists;
        }

        private async void GetSearch()
        {
            string query = txtQuery.Text;
            if (!string.IsNullOrEmpty(query))
            {
                List<SearchListItem> results = await SearchRequest.GetAll(query);
                SearchResults.DataContext = null;
                SearchResults.DataContext = results;
            }
        }

        private void txtQuery_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                GetSearch();
        }

        private void TopSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = TopSongs.SelectedItem as SongListItem;
            this.Frame.Navigate(typeof(SongDetailsPage), selected.Id);
        }

        private void TopAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = TopAlbums.SelectedItem as AlbumListItem;
            this.Frame.Navigate(typeof(AlbumDetailsPage), selected.Id);
        }

        private void Playlists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Playlists.SelectedItem as Playlist;
            this.Frame.Navigate(typeof(PlaylistDetailsPage), selected.Id);
        }

        private void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = SearchResults.SelectedItem as SearchListItem;
            if (selected.Type == "Artist")
                this.Frame.Navigate(typeof(ArtistDetailsPage), selected.Id);
            else if (selected.Type == "Album")
                this.Frame.Navigate(typeof(AlbumDetailsPage), selected.Id);
            else if (selected.Type == "Song")
                this.Frame.Navigate(typeof(SongDetailsPage), selected.Id);
        }

    }
}
