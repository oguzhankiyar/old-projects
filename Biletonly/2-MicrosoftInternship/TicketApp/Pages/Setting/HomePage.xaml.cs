using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages.Setting
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Ayarlar");
        }

        private void btnResetTickets_Clicked(object sender, EventArgs e)
        {
            Database.SavedData.Tickets = new List<Ticket>();
            App.ShowProgress("biletler sıfırlandı", ProgressType.Success, ProgressTime.Normal);
        }

        private void btnUpdateStations_Clicked(object sender, EventArgs e)
        {
            Database.SavedData.StationsUpdatedDate = DateTime.Now.AddHours((-1 * Constraints.StationsMinimumUpdateHour) - 1);
            Database.SavedData.Stations = new List<Station>();
            Database.SavedData.Airports = new List<Station>();
            Functions.UpdateStations();
            App.ShowProgress("istasyonlar güncelleniyor...");
            StationRequests.AirportsRequest.OnCompleted += (response) =>
            {
                if (StationRequests.FromStationsRequest.IsCompleted && response.Status == ResponseStatus.Successful)
                    App.ShowProgress("istasyonlar güncellendi", ProgressType.Success, ProgressTime.Normal);
                else if (response.Status != ResponseStatus.Successful)
                    App.ShowProgress("istasyonlar güncellenemedi", ProgressType.Error, ProgressTime.Normal);
            };
            StationRequests.FromStationsRequest.OnCompleted += (response) =>
            {
                if (StationRequests.AirportsRequest.IsCompleted && response.Status == ResponseStatus.Successful)
                    App.ShowProgress("istasyonlar güncellendi", ProgressType.Success, ProgressTime.Normal);
                else if (response.Status != ResponseStatus.Successful)
                    App.ShowProgress("istasyonlar güncellenemedi", ProgressType.Error, ProgressTime.Normal);
            };
        }

        private void btnResetFavoritePassengers_Clicked(object sender, EventArgs e)
        {
            Database.SavedData.FavoritePassengers = new List<Passenger>();
            App.ShowProgress("favori yolcular sıfırlandı", ProgressType.Success, ProgressTime.Normal);
        }

        private void btnResetSearchHistory_Clicked(object sender, EventArgs e)
        {
            Database.SavedData.InitSavedSearch();
            App.ShowProgress("arama geçmişi sıfırlandı", ProgressType.Success, ProgressTime.Normal);
        }

        private void btnFavoritePassengers_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Setting/FavoritePassengerListPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}