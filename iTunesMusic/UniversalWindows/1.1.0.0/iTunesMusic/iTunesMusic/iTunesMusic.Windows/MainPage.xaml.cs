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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iTunesMusic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int page = 0;
        public MainPage()
        {
            this.InitializeComponent();
        }
        public async void metod()
        {
            //LayoutRoot.DataContext = await SearchRequest.GetAll("serdar ortaç");
            LayoutRoot.DataContext = null;
            LayoutRoot.DataContext = await SongRequest.GetSources(query.Text, page);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SongSource source = ListView1.SelectedItem as SongSource;
            string url = string.Format(Constants.URLs.SongSourceFile, source.Source);
            Player.Source = new Uri(url);
            Player.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            page++;
            metod();
        }
    }
}
