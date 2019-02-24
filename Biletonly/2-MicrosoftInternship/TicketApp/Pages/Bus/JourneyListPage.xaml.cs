using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages.Bus
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
            App.SetTitle("Uygun Seferler");

            JourneyList.ItemsSource = Database.TempData.Journeys.ToList();
            JourneyList.SelectedItem = null;
            JourneyRequests.JourneysRequest.OnCompleted = (response) =>
            {
                var journeys = response.Result;
                Database.TempData.Journeys = journeys;
                JourneyList.ItemsSource = journeys.ToList();
                JourneyList.SelectedItem = null;
            };
        }

        private void JourneyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = JourneyList.SelectedItem as Journey;
            if (JourneyList.SelectedItem != null && !selected.IsFull)
            {
                Logger.SelectionChanged("JourneyList", JourneyList.SelectedItem);
                if (!App.IsInternetAvailable)
                    App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
                else
                {
                    App.ShowProgress("sefer detayları yükleniyor...");
                    Database.TempData.Ticket.Journeys = new List<Journey>() { selected.Clone() };
                    Database.TempData.Ticket.Passengers = new List<Passenger>();
                    Database.TempData.SelectedGender = null;

                    BusRequests.BusRequest.OnCompleted = (response) =>
                    {
                        var bus = response.Result;
                        App.HideProgress();
                        if (bus != null)
                        {
                            bus.Properties = selected.Segments[0].Bus.Properties;
                            Database.TempData.Ticket.Is3DBuyingRequired = bus.Is3DSecureRequired;
                            Database.TempData.Ticket.Is3DBuyingActivated = bus.Is3DBuyingActivated;
                            Database.TempData.Ticket.Price = Database.TempData.Ticket.Journeys[0].Price;
                            Database.TempData.Ticket.Journeys[0].Segments[0].Bus = bus;
                            NavigationService.Navigate(new Uri("/Pages/Bus/JourneyDetailsPage.xaml", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            App.ShowProgress("sefer detayları yüklenemedi", ProgressType.Error, ProgressTime.Normal);
                        }
                    };
                    BusRequests.GetBus(selected.Segments[0]);
                }
                JourneyList.SelectedItem = null;
            }
        }

    }
}