#define test
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;
using Biletall.Helper.Requests;
using System.Windows.Input;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using TicketApp.Controls;
using TicketApp.Controls.Enums;

namespace TicketApp.Pages.Bus
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Otobüs Bileti");
            Database.TempData.Ticket = new Ticket();

            //txtFromStation.ValueChanged = txtFromStation_ValueChanged;
            var timeSpan = (TimeSpan)(DateTime.Now - Database.SavedData.StationsUpdatedDate);
            if (timeSpan.TotalHours < Constraints.StationsMinimumUpdateHour || !App.IsInternetAvailable)
            {
                txtFromStation.ItemsSource = txtToStation.ItemsSource = Database.SavedData.Stations.ToList();
            }
            else
            {
                App.ShowProgress("istasyonlar yükleniyor...");
                txtFromStation.ItemsSource = txtToStation.ItemsSource = Database.SavedData.Stations.ToList();
                StationRequests.FromStationsRequest.OnCompleted = (response) =>
                {
                    var stations = response.Result;
                    App.HideProgress();
                    Database.SavedData.Stations = stations;
                    txtFromStation.ItemsSource = txtToStation.ItemsSource = stations.ToList();
                };
                StationRequests.GetFromStations();
            }
            if (Database.SavedData.BusSearch != null)
            {
                SearchForm.DataContext = Database.SavedData.BusSearch;
                //txtFromStation_ValueChanged(null, null);
            }
        }

        /*private void txtFromStation_ValueChanged(object sender, EventArgs e)
        {
            if (txtFromStation.Value != null && App.IsInternetAvailable)
            {
                var fromValue = txtFromStation.Value as Station;
                var fromStation = Database.SavedData.Stations.SingleOrDefault(x => x.Name == fromValue.Name);
                if (fromStation != null)
                {
                    App.ShowProgress("varış istasyonları yükleniyor...");
                    StationRequest.OnToStationsCompleted = (stations) =>
                    {
                        if (stations != null)
                        {
                            txtToStation.ItemsSource = stations.ToList();
                            if (Database.SavedData.BusJourney != null && Database.SavedData.BusJourney.To != null && fromValue.Name.ToString() != Database.SavedData.BusJourney.To.Name.ToString())
                                txtToStation.Value = Database.SavedData.BusJourney.To;
                        }
                        App.HideProgress();
                    };
                    StationRequest.GetToStations(fromStation);
                }
            }
        }*/
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Logger.Clicked("btnSearch");

            this.Focus();
            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else if (!Database.SavedData.Stations.Any())
                App.ShowProgress("istasyonların yüklenmesini bekleyiniz", ProgressType.Warning, ProgressTime.Normal);
            else if (txtFromStation.Value == null || txtToStation.Value == null || dpDepartureDate.Value == null)
                App.ShowProgress("boş bıraktığınız alan(lar) var", ProgressType.Warning, ProgressTime.Normal);
            else if (dpDepartureDate.Value < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                App.ShowProgress("seçtiğiniz tarih bugünden geçmişte olamaz", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                App.AddBackPressedEvent(BackPressed);
                App.ShowProgress("uygun seferler aranıyor...");
                btnSearch.IsActivated = false;
                Database.TempData.Ticket.Type = TicketType.BusJourney;
                Database.TempData.TicketSearch = Database.SavedData.BusSearch = new TicketSearch()
                    {
                        Factory = new Factory() { Id = 0 },
                        From = txtFromStation.Value as Station,
                        To = txtToStation.Value as Station,
                        DepartureDate = Convert.ToDateTime(dpDepartureDate.Value),
                        Passengers = { new Passenger() },
                        Type = Database.TempData.Ticket.Type
                    };

                JourneyRequests.JourneysRequest.OnCompleted = (response) =>
                {
                    var journeys = response.Result;
                    App.RemoveBackPressedEvent(BackPressed);
                    Database.TempData.Journeys = journeys;
                    App.HideProgress();
                    if (journeys == null || journeys.Count() == 0)
                        App.ShowProgress("aradığınız kriterlere uygun sefer bulunamadı", ProgressType.Error, ProgressTime.Normal);
                    else
                        NavigationService.Navigate(new Uri("/Pages/Bus/JourneyListPage.xaml", UriKind.RelativeOrAbsolute));
                    btnSearch.IsActivated = true;
                };
                JourneyRequests.GetJourneys(Database.TempData.TicketSearch);
            }
        }

        private void BackPressed()
        {
            App.RemoveBackPressedEvent(BackPressed);
            JourneyRequests.JourneysRequest.Cancel();
            App.HideProgress();
            btnSearch.IsActivated = true;
        }
    }
}