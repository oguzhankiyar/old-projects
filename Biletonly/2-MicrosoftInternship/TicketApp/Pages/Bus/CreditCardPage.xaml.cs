using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages.Bus
{
    public partial class CreditCardPage : PhoneApplicationPage
    {
        public CreditCardPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Kredi Kartı Bilgileri");
            LayoutRoot.DataContext = Database.TempData.Ticket;

            if (!Database.TempData.Ticket.Is3DBuyingActivated)
            {
                cbUse3DSecure.IsEnabled = false;
                use3DPanel.Tap += (c, r) =>
                {
                    App.ShowProgress("bu işlem için 3D Secure ile ödeme aktif değil", ProgressType.Warning, ProgressTime.Normal);
                };
            }
            else if (Database.TempData.Ticket.Is3DBuyingRequired)
            {
                cbUse3DSecure.IsChecked = true;
                cbUse3DSecure.IsEnabled = false;
                use3DPanel.Tap += (c, r) =>
                {
                    App.ShowProgress("bu işlem için sadece 3D Secure ile ödeme yapılabiliyor", ProgressType.Warning, ProgressTime.Normal);
                };
            }
        }

        private void BuyingRules_Tap(object sender, GestureEventArgs e)
        {
            Logger.Clicked("BuyingRules");
            string rules = Constraints.Laws;
            App.ShowProgress(rules, ProgressType.Plain, ProgressTime.Infinite, new List<string>() { "Kabul et", "Vazgeç" });
            Header.HeaderConfirmed = (buttonId) =>
            {
                if (buttonId == 0)
                    cbAcceptRules.IsChecked = true;
                else
                    cbAcceptRules.IsChecked = false;
            };
        }

        private void btnBuy_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnBuy");
            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else if (string.IsNullOrEmpty(txtOwnerName.Value) || string.IsNullOrEmpty(txtCardNumber.Value) ||
                string.IsNullOrEmpty(txtExpirationMonth.Value) || string.IsNullOrEmpty(txtExpirationYear.Value) || string.IsNullOrEmpty(txtCVC.Value))
                App.ShowProgress("boş bıraktığınız alan(lar) var", ProgressType.Warning, ProgressTime.Normal);
            else if (txtCardNumber.Length != 16)
                App.ShowProgress("kart numarası 16 haneden oluşmalıdır", ProgressType.Warning, ProgressTime.Normal);
            else if (cbAcceptRules.IsChecked != true)
                App.ShowProgress("kuralları okuyup, kabul ettiğinizi belirtmelisiniz", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                var ticket = Database.TempData.Ticket;
                if (cbUse3DSecure.IsChecked == true)
                {
                    NavigationService.Navigate(new Uri("/Pages/Buying3DSecurePage.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    btnBuy.IsActivated = false;
                    App.AddBackPressedEvent(BackPressed);
                    BuyingRequests.BuyingRequest.OnCompleted = (response) =>
                    {
                        var actionResult = response.Result;
                        btnBuy.IsActivated = true;
                        App.RemoveBackPressedEvent(BackPressed);
                        if (response.Status == ResponseStatus.Successful)
                        {
                            Database.FillSavedPNR();
                            Database.AddPassenger(Database.TempData.Ticket.Passengers.ToList());

                            Database.TempData.Ticket.PNR.Code = actionResult.PNR;
                            Database.TempData.Ticket.PNR.ExpirationDate = actionResult.ExpirationDate;
                            Database.SavedData.Tickets.Add(Database.TempData.Ticket);

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
                    BuyingRequests.GetBuying(ticket);
                }
            }
        }

        private void BackPressed()
        {
            return;
        }
    }
}