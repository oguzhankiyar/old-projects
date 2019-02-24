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
using TicketApp.Models;
using TicketApp.UserControls;

namespace TicketApp.Pages.Airplane
{
    public partial class JourneyListPage : PhoneApplicationPage
    {
        public JourneyListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Uygun Uçuşlar");
            
            if (NavigationContext.QueryString.ContainsKey("returnFlights"))
            {
                FlightList.ItemsSource = Database.TempData.Journeys.Where(x => x.IsReturn == true).ToList();
                JourneyFilterPanel.IsReturn = true;
            }
            else
            {
                FlightList.ItemsSource = Database.TempData.Journeys.Where(x => x.IsReturn == false).ToList();
                JourneyFilterPanel.IsReturn = false;
            }
            FlightList.SelectedItem = null;

            if (Database.TempData.TicketSearch.JourneyType == JourneyType.RoundTrip)
            {
                TitleText.Visibility = Visibility.Visible;
                if (NavigationContext.QueryString.ContainsKey("returnFlights"))
                    TitleText.Value = "Dönüş seferleri";
                else
                    TitleText.Value = "Gidiş seferleri";
            }

            JourneyRequests.JourneysRequest.OnCompleted = (response) =>
            {
                var journeys = response.Result;
                Database.TempData.Journeys = journeys;
                if (NavigationContext.QueryString.ContainsKey("returnFlights"))
                    FlightList.ItemsSource = journeys.Where(x => x.IsReturn == true).ToList();
                else
                    FlightList.ItemsSource = journeys.Where(x => x.IsReturn == false).ToList();
                FlightList.SelectedItem = null;
            };
        }

        private void FlightList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FlightList.SelectedItem != null && !(FlightList.SelectedItem as Journey).IsFull)
            {
                Logger.SelectionChanged("FlightList", FlightList.SelectedItem);
                if (Database.TempData.TicketSearch.JourneyType == JourneyType.RoundTrip && !NavigationContext.QueryString.ContainsKey("returnFlights"))
                {
                    App.ShowProgress("dönüş uçuşları yükleniyor...");
                    FlightListItem.OnCompleted = () =>
                    {
                        Database.TempData.Ticket.Journeys = new List<Journey>() { (FlightList.SelectedItem as Journey).Clone() };
                        Database.TempData.Ticket.Passengers = Functions.ClonePassengers(Database.TempData.TicketSearch.Passengers);
                        App.HideProgress();
                        NavigationService.Navigate(new Uri("/Pages/Airplane/JourneyListPage.xaml?returnFlights=true", UriKind.RelativeOrAbsolute));
                    };
                }
                else
                {
                    App.ShowProgress("uçuş detayları yükleniyor...");
                    FlightListItem.OnCompleted = () =>
                    {
                        if (NavigationContext.QueryString.ContainsKey("returnFlights"))
                        {
                            Database.TempData.Ticket.Journeys.RemoveAll(x => x.IsReturn);
                            Database.TempData.Ticket.Journeys.Add(FlightList.SelectedItem as Journey);
                        }
                        else
                            Database.TempData.Ticket.Journeys = new List<Journey>() { (FlightList.SelectedItem as Journey).Clone() };

                        Database.TempData.Ticket.Passengers = Functions.ClonePassengers(Database.TempData.TicketSearch.Passengers);
                        App.HideProgress();
                        NavigationService.Navigate(new Uri("/Pages/Airplane/JourneyDetailsPage.xaml", UriKind.RelativeOrAbsolute));
                    };
                }
            }
        }
    }
}