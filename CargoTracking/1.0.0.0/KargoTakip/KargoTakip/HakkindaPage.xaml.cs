using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KargoTakip.Models;
using System.Windows.Input;

namespace KargoTakip
{
    public partial class HakkindaPage : PhoneApplicationPage
    {
        public HakkindaPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Data.Gecmis.Count() == 0)
                Gecmis_Uyari.Visibility = Visibility.Visible;
            GecmisLLS.ItemsSource = Data.Gecmis.OrderByDescending(x => x.SonGiris).ToList();
            base.OnNavigatedTo(e);
        }
        private void BackImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/TakipPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void GecmisLLS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Takip takip = GecmisLLS.SelectedItem as Takip;
            NavigationService.Navigate(new Uri(string.Format("/TakipPage.xaml?SirketAdi={0}&Kod={1}", takip.Sirket.Adi, takip.Kod), UriKind.RelativeOrAbsolute));
        }

        private void GecmistenSil_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext as Takip;
            Function.GecmistenSil(selected);
            GecmisLLS.ItemsSource = Data.Gecmis;
            if (Data.Gecmis.Count() == 0)
                Gecmis_Uyari.Visibility = Visibility.Visible;
        }
    }
}