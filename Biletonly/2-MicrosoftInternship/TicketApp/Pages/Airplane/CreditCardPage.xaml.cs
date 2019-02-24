using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Biletall.Helper.Entities;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages.Airplane
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
            if (Database.TempData.Ticket.Bill.Type == BillType.Person)
                cbPersonBill.IsChecked = true;
            else
                cbFactoryBill.IsChecked = true;
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
            else if (string.IsNullOrEmpty(txtOwnerName.Value) || string.IsNullOrEmpty(txtCardNumber.Value) || string.IsNullOrEmpty(txtExpirationMonth.Value) || string.IsNullOrEmpty(txtExpirationYear.Value) || string.IsNullOrEmpty(txtCVC.Value) || 
                (cbPersonBill.IsChecked == true && (txtBillTCKN.Value == "0" || string.IsNullOrEmpty(txtBillFirstName.Value) || string.IsNullOrEmpty(txtBillLastName.Value))) ||
                (cbFactoryBill.IsChecked == true && (string.IsNullOrEmpty(txtBillFactoryName.Value) || string.IsNullOrEmpty(txtBillTaxId.Value) || string.IsNullOrEmpty(txtBillTaxOfficeName.Value))))
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
                        if (Database.TempData.Ticket.Bill.Type == BillType.Person)
                        {
                            Database.AddPassenger(new Passenger()
                            {
                                TCKN = Database.TempData.Ticket.Bill.PersonTCKN,
                                FirstName = Database.TempData.Ticket.Bill.PersonFirstName,
                                LastName = Database.TempData.Ticket.Bill.PersonLastName,
                                Address = Database.TempData.Ticket.Bill.Address
                            });
                        }
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

        private void BillType_Checked(object sender, RoutedEventArgs e)
        {
            Logger.Clicked("BillType");
            var selected = sender as RadioButton;
            if (selected == cbPersonBill)
            {
                Database.TempData.Ticket.Bill.Type = BillType.Person;
                OpenFavoriesText.Visibility = Visibility.Visible;
                FactoryBillPanel.Visibility = Visibility.Collapsed;
                PersonBillPanel.Visibility = Visibility.Visible;
            }
            else if (selected == cbFactoryBill)
            {
                Database.TempData.Ticket.Bill.Type = BillType.Factory;
                OpenFavoriesText.Visibility = Visibility.Collapsed;
                PersonBillPanel.Visibility = Visibility.Collapsed;
                FactoryBillPanel.Visibility = Visibility.Visible;
            }
        }

        private void OpenFavorites_Tap(object sender, EventArgs e)
        {
            Logger.Clicked("OpenFavorites");
            var passengers = Database.SavedData.FavoritePassengers.Where(x => !(txtBillFirstName.Value == x.FirstName && txtBillLastName.Value == x.LastName)).OrderBy(x => x.FirstName).ToList();
            if (passengers.Count() > 0)
            {
                FavoritePassengerListPicker.ItemsSource = passengers;
                FavoritePassengerListPicker.Open();
            }
            else
            {
                App.ShowProgress("henüz kayıtlı yolcu yok, ayarlardan ekleyebilirsiniz", ProgressType.Error, ProgressTime.Normal);
            }
        }

        private void FavoritePassengers_ValueChanged(object sender, EventArgs e)
        {
            var passenger = FavoritePassengerListPicker.Value as Passenger;
            if (passenger != null)
            {
                txtBillTCKN.Value = passenger.TCKN.ToString();
                txtBillFirstName.Value = passenger.FirstName;
                txtBillLastName.Value = passenger.LastName;
                txtBillAddress.Value = passenger.Address;
            }
        }
    }
}