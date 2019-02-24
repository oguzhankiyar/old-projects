using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iTunes.Models;

namespace iTunes
{
    public partial class InstallationPage : PhoneApplicationPage
    {
        public InstallationPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            if (Database.Settings.InstallationCompleted == true)
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            Countries.ItemsSource = Database.Countries;
            base.OnNavigatedTo(e);
        }
        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Database.Settings.DefaultCountry = Countries.SelectedItem as Country;
            Database.Settings.InstallationCompleted = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}