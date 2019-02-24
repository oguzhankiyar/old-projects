using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using iTunesMusic.Entities;
using iTunesMusic.Entities.EntityListItems;
using iTunesMusic.Tools;
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
    public sealed partial class PlaylistDetailsPage : Page
    {
        private Playlist currentPlaylist;

        public PlaylistDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int id = Convert.ToInt32(e.Parameter.ToString());
            GetPlaylistDetails(id);
        }

        private void GetPlaylistDetails(int id)
        {
            currentPlaylist = Database.Playlists.SingleOrDefault(x => x.Id == id);
            PlaylistDetails.DataContext = currentPlaylist;
        }

        private void Songs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Songs.SelectedItem as SongListItem;
            if (selected != null)
                this.Frame.Navigate(typeof(SongDetailsPage), selected.Id);
        }
    }
}
