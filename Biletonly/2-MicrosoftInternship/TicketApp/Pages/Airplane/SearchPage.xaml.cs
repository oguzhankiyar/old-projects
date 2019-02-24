using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;
using Biletall.Helper.Requests;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using System.Text;
using TicketApp.Controls;
using TicketApp.Controls.Enums;

namespace TicketApp.Pages.Airplane
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();

            Database.TempData.Ticket = new Ticket();
            rbOneWay.IsChecked = true;
            if (Database.SavedData.AirplaneSearch != null)
            {
                SearchForm.DataContext = Database.TempData.TicketSearch = Database.SavedData.AirplaneSearch;
                Database.TempData.Ticket.Type = Database.SavedData.AirplaneSearch.Type;
                if (Database.SavedData.AirplaneSearch.JourneyType == JourneyType.RoundTrip)
                    rbRoundTrip.IsChecked = true;
            }
            txtFromStation.ValueChanged += txtStation_ValueChanged;
            txtToStation.ValueChanged += txtStation_ValueChanged;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Uçak Bileti");

            var timeSpan = (TimeSpan)(DateTime.Now - Database.SavedData.StationsUpdatedDate);
            if (timeSpan.TotalHours < Constraints.StationsMinimumUpdateHour || !App.IsInternetAvailable)
            {
                txtFromStation.ItemsSource = txtToStation.ItemsSource = Database.SavedData.Airports.ToList();
            }
            else
            {
                txtFromStation.ItemsSource = txtToStation.ItemsSource = Database.SavedData.Airports.ToList();
                App.ShowProgress("havalimanları yükleniyor...");
                StationRequests.AirportsRequest.OnCompleted = (response) =>
                {
                    var airports = response.Result;
                    App.HideProgress();
                    Database.SavedData.Airports = airports;
                    txtFromStation.ItemsSource = txtToStation.ItemsSource = airports.ToList();
                };
            }
            ControlPassengers();
        }

        private void txtStation_ValueChanged(object sender, EventArgs e)
        {
            Logger.ValueChanged("txtFromStation", txtFromStation.Value);
            Logger.ValueChanged("txtToStation", txtToStation.Value);

            var fromStation = txtFromStation.Value as Station;
            var toStation = txtToStation.Value as Station;
            if ((fromStation != null && fromStation.IsInternational) || (toStation != null && toStation.IsInternational))
                Database.TempData.Ticket.Type = TicketType.InternationalJourney;
            else
                Database.TempData.Ticket.Type = TicketType.AirplaneJourney;

            ControlPassengers();
        }
        
        private void ControlPassengers()
        {
            var fromStation = txtFromStation.Value as Station;
            var toStation = txtToStation.Value as Station;
            if (Database.TempData.Ticket.Type == TicketType.InternationalJourney)
            {
                var passengers = Database.TempData.TicketSearch.Passengers.Where(x => x.Type == PassengerType.Infant ||
                                                                    x.Type == PassengerType.Student ||
                                                                    x.Type == PassengerType.Military ||
                                                                    x.Type == PassengerType.Teenager).ToList();
                foreach (var p in passengers)
                    p.Type = PassengerType.Adult;
            }
            txtPassengers.Value = Functions.FillPassengersText(Database.TempData.TicketSearch.Passengers);
        }

        private void JourneyType_Checked(object sender, RoutedEventArgs e)
        {
            if (rbRoundTrip.IsChecked == true)
            {
                Database.TempData.TicketSearch.JourneyType = JourneyType.RoundTrip;
                Database.TempData.TicketSearch.ReturnDate = Database.TempData.TicketSearch.DepartureDate.AddDays(2);
                dpReturnDate.Visibility = Visibility.Visible;
                if ((DateTime)dpReturnDate.Value < (DateTime)dpDepartureDate.Value)
                    dpReturnDate.Value = ((DateTime)dpDepartureDate.Value).AddDays(7);
                Grid.SetColumnSpan(dpDepartureDate, 1);
            }
            else
            {
                Database.TempData.TicketSearch.JourneyType = JourneyType.OneWay;
                dpReturnDate.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(dpDepartureDate, 2);
            }
        }

        private void Passengers_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Logger.Clicked("Passengers");
            var fromStation = txtFromStation.Value as Station;
            var toStation = txtToStation.Value as Station;
            if (Database.TempData.Ticket.Type == TicketType.InternationalJourney)
                NavigationService.Navigate(new Uri("/Pages/Airplane/SelectPassengerPage.xaml?internationalFlight=true", UriKind.RelativeOrAbsolute));
            else
                NavigationService.Navigate(new Uri("/Pages/Airplane/SelectPassengerPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Logger.Clicked("btnSearch");
            this.Focus();
            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else if (!Database.SavedData.Airports.Any())
                App.ShowProgress("havalimanlarının yüklenmesini bekleyiniz", ProgressType.Warning, ProgressTime.Normal);
            else if (txtFromStation.Value == null || txtToStation.Value == null || dpDepartureDate.Value == null || (rbRoundTrip.IsChecked == true && dpReturnDate.Value == null))
                App.ShowProgress("boş bıraktığınız alan(lar) var", ProgressType.Warning, ProgressTime.Normal);
            else if (dpDepartureDate.Value < Convert.ToDateTime(DateTime.Now.ToShortDateString()) || (rbRoundTrip.IsChecked == true && dpReturnDate.Value < Convert.ToDateTime(DateTime.Now.ToShortDateString())))
                App.ShowProgress("seçtiğiniz tarih bugünden geçmişte olamaz", ProgressType.Warning, ProgressTime.Normal);
            else if (rbRoundTrip.IsChecked == true && dpDepartureDate.Value > dpReturnDate.Value)
                App.ShowProgress("dönüş tarihi gidiş tarihinden önce olamaz", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                App.AddBackPressedEvent(BackPressed);
                Database.TempData.TicketSearch.From = txtFromStation.Value as Station;
                Database.TempData.TicketSearch.To = txtToStation.Value as Station;
                Database.TempData.TicketSearch.DepartureDate = Convert.ToDateTime(dpDepartureDate.Value);
                Database.TempData.TicketSearch.ReturnDate = Convert.ToDateTime(dpReturnDate.Value);
                
                Database.TempData.TicketSearch.Type = Database.TempData.Ticket.Type;
                App.ShowProgress("uygun seferler aranıyor...");
                btnSearch.IsActivated = false;

                Database.SavedData.AirplaneSearch = Database.TempData.TicketSearch;

                JourneyRequests.JourneysRequest.OnCompleted = (response) =>
                {
                    var flights = response.Result;
                    App.RemoveBackPressedEvent(BackPressed);
                    Database.TempData.Journeys = flights;
                    App.HideProgress();
                    if (flights == null || flights.Count() == 0)
                        App.ShowProgress("aradığınız kriterlere uygun sefer bulunamadı", ProgressType.Error, ProgressTime.Normal);
                    else
                        NavigationService.Navigate(new Uri("/Pages/Airplane/JourneyListPage.xaml", UriKind.RelativeOrAbsolute));
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