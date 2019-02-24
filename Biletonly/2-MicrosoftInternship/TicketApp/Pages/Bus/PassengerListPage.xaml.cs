using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
    public partial class PassengerListPage : PhoneApplicationPage
    {
        public PassengerListPage()
        {
            InitializeComponent();
            if (Database.SavedData.PNR != null)
                Database.TempData.Ticket.PNR = Database.SavedData.PNR;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Yolcu Detayları");

            LayoutRoot.DataContext = Database.TempData.Ticket;
            
            if (Database.TempData.Ticket.ActionType == ActionType.Reservation)
                btnContinue.Value = "Rezervasyonu tamamla";
            else
                btnContinue.Value = "İşleme devam et";
        }

        private void btnContinue_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnContinue");
            bool isValid = true;
            foreach (var passenger in Database.TempData.Ticket.Passengers.ToList())
                if (!passenger.IsValidForBus)
                    isValid = false;
            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else if (!isValid || string.IsNullOrEmpty(txtEmail.Value) || string.IsNullOrEmpty(txtPhoneNumber.Value))
                App.ShowProgress("boş bıraktığınız alan(lar) var", ProgressType.Warning, ProgressTime.Normal);
            else if (!Regex.Match(txtEmail.Value.ToString(), @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$").Success)
                App.ShowProgress("email adresiniz geçerli değil", ProgressType.Warning, ProgressTime.Normal);
            else if (txtPhoneNumber.Value.Length < 10)
                App.ShowProgress("telefon numaranız 10 haneden oluşmalıdır", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                if (Database.TempData.Ticket.ActionType == ActionType.Reservation)
                {
                    App.AddBackPressedEvent(BackPressed);
                    btnContinue.IsActivated = false;
                    App.ShowProgress("rezervasyon yapılıyor...");
                    ReservationRequests.ReservationRequest.OnCompleted = (response) =>
                    {
                        var actionResult = response.Result;
                        App.HideProgress();
                        btnContinue.IsActivated = true;
                        App.RemoveBackPressedEvent(BackPressed);
                        if (response.Status == ResponseStatus.Successful)
                        {
                            Database.AddPassenger(Database.TempData.Ticket.Passengers.ToList());

                            Database.TempData.Ticket.PNR.Code = actionResult.PNR;
                            Database.TempData.Ticket.PNR.Parameter = Database.TempData.Ticket.Passengers[0].LastName;
                            Database.TempData.Ticket.PNR.ExpirationDate = actionResult.ExpirationDate;
                            Database.SavedData.Tickets.Add(Database.TempData.Ticket);
                            Database.FillSavedPNR();

                            Functions.UpdateTicket(Database.TempData.Ticket.PNR, NavigationService);
                            App.ClearBackHistory();
                        }
                        else if (response.Status == ResponseStatus.InvalidTCKN)
                        {
                            App.ShowProgress("belirttiğiniz yolcuların bilgilerini kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
                        }
                        else
                        {
                            App.ShowProgress(response.Message, ProgressType.Error, ProgressTime.Normal);
                        }
                    };
                    ReservationRequests.GetReservation(Database.TempData.Ticket);
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Pages/Bus/CreditCardPage.xaml", UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void BackPressed()
        {
            return;
        }
    }
}