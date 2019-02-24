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

namespace TicketApp.Pages.Bus
{
    public partial class SeatListPage : PhoneApplicationPage
    {
        public SeatListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Koltuk Seçimi");

            var bus = Database.TempData.Ticket.Journeys[0].Segments[0].Bus;
            if (SeatsGrid.Children.Count() == 0)
                Functions.CreateSeats(SeatsGrid, bus.Seats);
        }

        private void btnReserve_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnReserve");
            Database.TempData.Ticket.ActionType = ActionType.Reservation;

            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else if (!Database.TempData.Ticket.Passengers.Any())
                App.ShowProgress("devam etmek için koltuk seçimi yapınız", ProgressType.Warning, ProgressTime.Normal);
            else if (Database.TempData.Ticket.Journeys[0].MaxReservationDate < DateTime.Now)
                App.ShowProgress("bu sefer için rezervasyon işlemi kapatılmıştır", ProgressType.Warning, ProgressTime.Normal);
            else
                IsSeatsEmpty();
        }

        private void btnBuy_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnBuy");
            Database.TempData.Ticket.ActionType = ActionType.Buying;

            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else if (!Database.TempData.Ticket.Passengers.Any())
                App.ShowProgress("devam etmek için koltuk seçimi yapınız", ProgressType.Warning, ProgressTime.Normal);
            else
                IsSeatsEmpty();
        }

        private void IsSeatsEmpty()
        {
            App.ShowProgress("işlem gerçekleştiriliyor..");
            btnReserve.IsActivated = false;
            btnBuy.IsActivated = false;

            BusRequests.SeatStatesRequest.OnCompleted = (response) =>
            {
                var seatStates = response.Result;
                var filledSeats = seatStates.Where(x => x.State != SeatState.Empty);
                if (filledSeats.Any())
                {
                    string seatString = "";
                    foreach (var seat in filledSeats.ToList())
                        seatString += seat.Number + ", ";

                    seatString = seatString.TrimEnd(' ').TrimEnd(',');
                    App.ShowProgress(seatString + " numaralı koltuk(lar) başkası tarafından alınmış, farklı bir koltuk seçiniz", ProgressType.Error, ProgressTime.Normal);

                    btnReserve.IsActivated = true;
                    btnBuy.IsActivated = true;
                }
                else
                {
                    GetServiceStops();
                }
            };
            BusRequests.GetSeatStates(Database.TempData.Ticket);
        }

        private void GetServiceStops()
        {
            ServiceRequests.FromServicesRequest.OnCompleted = (response) =>
            {
                var fromServices = response.Result;
                Database.TempData.FromServices = fromServices;
                if (ServiceRequests.ToServicesRequest.IsCompleted)
                {
                    btnReserve.IsActivated = true;
                    btnBuy.IsActivated = true;

                    App.HideProgress();
                    NavigationService.Navigate(new Uri("/Pages/Bus/PassengerListPage.xaml", UriKind.RelativeOrAbsolute));
                }
            };
            ServiceRequests.ToServicesRequest.OnCompleted = (response) =>
            {
                var toServices = response.Result;
                Database.TempData.ToServices = toServices;
                if (ServiceRequests.FromServicesRequest.IsCompleted)
                {
                    btnReserve.IsActivated = true;
                    btnBuy.IsActivated = true;

                    App.HideProgress();
                    NavigationService.Navigate(new Uri("/Pages/Bus/PassengerListPage.xaml", UriKind.RelativeOrAbsolute));
                }
            };
            ServiceRequests.GetFromServices(Database.TempData.Ticket.Journeys[0].Segments[0]);
            ServiceRequests.GetToServices(Database.TempData.Ticket.Journeys[0].Segments[0]);
        }
    }
}