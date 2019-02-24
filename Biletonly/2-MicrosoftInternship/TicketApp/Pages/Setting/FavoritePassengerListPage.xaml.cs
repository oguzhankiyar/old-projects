using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;

namespace TicketApp.Pages.Setting
{
    public partial class FavoritePassengerListPage : PhoneApplicationPage
    {
        public FavoritePassengerListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Favori Yolcular");
            FavoritePassengerList.ItemsSource = null;
            FavoritePassengerList.ItemsSource = Database.SavedData.FavoritePassengers.ToList();
        }

        private void AddPassenger_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Setting/AddPassengerPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void DetailBox_Clicked(object sender, EventArgs e)
        {
            var selectedPassenger = FavoritePassengerList.SelectedItem as Passenger;
            NavigationService.Navigate(new Uri("/Pages/Setting/AddPassengerPage.xaml?name=" + selectedPassenger.FullName, UriKind.RelativeOrAbsolute));
        }
    }
}