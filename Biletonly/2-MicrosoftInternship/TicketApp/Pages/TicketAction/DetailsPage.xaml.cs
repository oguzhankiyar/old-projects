using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;
using TicketApp.Controls.Enums;
using Biletall.Helper.Entities;
using System.Windows.Media;
using TicketApp.UserControls;
using System.Windows.Threading;
using TicketApp.Controls;
using Microsoft.Phone.Tasks;

namespace TicketApp.Pages.TicketAction
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        private int _actionType = -1;
        List<Grid> _gridList = new List<Grid>();

        public DetailsPage()
        {
            InitializeComponent();
            ControlBillPanels();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Bilet Detayları");

            LayoutRoot.DataContext = Database.TempData.Ticket;
            
            if (Database.TempData.Ticket.Passengers.Count(x => x.LastAction.Type == ActionType.Reservation) == 0 &&
                Database.TempData.Ticket.Passengers.Count(x => x.LastAction.Type == ActionType.CanceledReservation) == 0)
                txtReservationOption.Visibility = Visibility.Collapsed;
            else if (Database.TempData.Ticket.PNR.ExpirationDate < DateTime.Now)
                txtReservationOption.Value = Database.TempData.Ticket.PNR.ExpirationDate.ToString("d MMMMMMM yyyy, ddd HH:mm") + " (Geçmiş)";

            // make these with converters
            txtTicketType.Value = Database.TempData.Ticket.Type.GetDescription();
            txtLastState.Value = Database.TempData.Ticket.GetLastState();
        }

        private void ControlBillPanels()
        {
            Logger.MethodCalled("DetailsPage.ControlBillPanels");
            if (!string.IsNullOrEmpty(Database.TempData.Ticket.Bill.PersonFirstName))
            {
                var bill = Database.TempData.Ticket.Bill;
                var billPanel = new StackPanel() { Margin = new Thickness(-12, -6, -12, 0) };
                billPanel.Children.Add(new DetailBox() { Label = "Kişi TC kimlik numarası:", Value = bill.PersonTCKN.ToString() });
                billPanel.Children.Add(new DetailBox() { Label = "Kişi adı:", Value = bill.PersonFirstName });
                billPanel.Children.Add(new DetailBox() { Label = "Kişi soyadı:", Value = bill.PersonLastName });
                billPanel.Children.Add(new DetailBox() { Label = "Kişi adresi:", Value = bill.Address });
                var pivotItem = new PivotItem()
                {
                    Header = new TextBlock() { Text = "fatura", FontSize = 40 },
                    Content = billPanel
                };
                TicketPivot.Items.Add(pivotItem);
            }
            if (!string.IsNullOrEmpty(Database.TempData.Ticket.Bill.FactoryName))
            {
                var bill = Database.TempData.Ticket.Bill;
                var billPanel = new StackPanel() { Margin = new Thickness(-12, -6, -12, 0) };
                billPanel.Children.Add(new DetailBox() { Label = "Firma adı:", Value = bill.FactoryName });
                billPanel.Children.Add(new DetailBox() { Label = "Firma vergi numarası:", Value = bill.FactoryTaxId });
                billPanel.Children.Add(new DetailBox() { Label = "Firma vergi dairesi adı:", Value = bill.FactoryTaxOfficeName });
                billPanel.Children.Add(new DetailBox() { Label = "Firma adresi:", Value = bill.Address });
                var pivotItem = new PivotItem()
                {
                    Header = new TextBlock() { Text = "fatura", FontSize = 40 },
                    Content = billPanel
                };
                TicketPivot.Items.Add(pivotItem);
            }
        }

        private void btnBuyTicket_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnBuyTicket");
            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Error, ProgressTime.Normal);
            else
            {
                TicketPivot.SelectedIndex = 2;
                var dispacterTimer = new DispatcherTimer();
                dispacterTimer.Tick += (c, r) =>
                {
                    dispacterTimer.Stop();
                    App.AddBackPressedEvent(BackPressed);
                    _actionType = 1;
                    DefaultButtons.Visibility = Visibility.Collapsed;
                    ContinueButtons.Visibility = Visibility.Visible;
                    TicketPassengerItem.CheckBoxesForBuy();
                };
                dispacterTimer.Interval = TimeSpan.FromMilliseconds(500);
                dispacterTimer.Start();
            }
        }

        private void btnCancelTicket_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnCancelTicket");
            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Error, ProgressTime.Normal);
            else if (Database.TempData.Ticket.Journeys.Any(x => x.Segments.Any(y => y.DepartureDate < DateTime.Now)))
                App.ShowProgress("yolculuk tarihi geçmiş", ProgressType.Error, ProgressTime.Normal);
            else
            {
                TicketPivot.SelectedIndex = 2;
                var dispacterTimer = new DispatcherTimer();
                dispacterTimer.Tick += (c, r) =>
                {
                    dispacterTimer.Stop();
                    App.AddBackPressedEvent(BackPressed);
                    _actionType = 2;
                    DefaultButtons.Visibility = Visibility.Collapsed;
                    ContinueButtons.Visibility = Visibility.Visible;
                    TicketPassengerItem.CheckBoxesForCancel();
                };
                dispacterTimer.Interval = TimeSpan.FromMilliseconds(500);
                dispacterTimer.Start();
            }
        }

        private void BuyTicket()
        {
            Logger.MethodCalled("DetailsPage.BuyTicket");
            btnContinue.IsActivated = true;
            btnBack.IsActivated = true;
            DefaultButtons.Visibility = Visibility.Visible;
            ContinueButtons.Visibility = Visibility.Collapsed;

            // İptal edilen veya satın alınan biletler hariç satın alma işlemi
            if (Database.TempData.Ticket.PNR.ExpirationDate < DateTime.Now)
            {
                TicketPassengerItem.HideCheckBoxes();
                App.ShowProgress("rezervasyon opsiyon tarihiniz geçmiş", ProgressType.Warning, ProgressTime.Normal);
            }
            else
            {
                foreach (var p in TicketPassengerItem.UncheckedPassengerList)
                    Database.TempData.Ticket.Passengers.Remove(p);

                TicketPassengerItem.HideCheckBoxes();

                if (Database.TempData.Ticket.Type == TicketType.BusJourney)
                    NavigationService.Navigate(new Uri("/Pages/Bus/CreditCardPage.xaml", UriKind.RelativeOrAbsolute));
                else
                    NavigationService.Navigate(new Uri("/Pages/Airplane/CreditCardPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void CancelTicket()
        {
            Logger.MethodCalled("DetailsPage.CancelTicket");
            if (Database.TempData.Ticket.Passengers.Count(x => x.LastAction.Type == ActionType.Buying) == 0) // (Database.TempData.Ticket.ActionType == ActionType.Reservation)
            {
                if (Database.TempData.Ticket.PNR.ExpirationDate < DateTime.Now)
                {
                    App.ShowProgress("rezervasyon opsiyon tarihiniz geçmiş", ProgressType.Warning, ProgressTime.Normal);
                    btnContinue.IsActivated = true;
                    btnBack.IsActivated = true;
                    DefaultButtons.Visibility = Visibility.Visible;
                    ContinueButtons.Visibility = Visibility.Collapsed;
                    TicketPassengerItem.HideCheckBoxes();
                }
                else
                {
                    int totalCount = Database.TempData.Ticket.Passengers.Count();
                    var checkedPassengers = TicketPassengerItem.CheckedPassengerList;
                    int checkedCount = checkedPassengers.Count();

                    if (checkedCount != totalCount && Database.TempData.Ticket.Type != TicketType.BusJourney)
                        App.ShowProgress("biletler hepsi birlikte iptal edilebilir", ProgressType.Error, ProgressTime.Normal);
                    else if (checkedCount != totalCount && checkedCount > 1)
                        App.ShowProgress("bileti teker teker iptal edebilirsiniz", ProgressType.Error, ProgressTime.Normal);
                    else
                    {
                        btnContinue.IsActivated = false;
                        btnBack.IsActivated = false;

                        foreach (var p in TicketPassengerItem.UncheckedPassengerList)
                            Database.TempData.Ticket.Passengers.Remove(p);

                        App.ShowProgress("bilet iptal edilecek, onaylıyor musunuz?", ProgressType.Plain, ProgressTime.Infinite, new List<string>() { "Evet, iptal et", "Hayır" });
                        Header.HeaderConfirmed = (buttonId) =>
                        {
                            App.HideProgress();
                            if (buttonId == 0)
                            {
                                Logger.Clicked("btnConfirm");
                                App.ShowProgress("işlem gerçekleştiriliyor..");
                                ReservationRequests.CancelReservationRequest.OnCompleted = (response) =>
                                {
                                    var actionResult = response.Result;
                                    btnContinue.IsActivated = true;
                                    btnBack.IsActivated = true;
                                    DefaultButtons.Visibility = Visibility.Visible;
                                    ContinueButtons.Visibility = Visibility.Collapsed;
                                    TicketPassengerItem.HideCheckBoxes();

                                    App.HideProgress();
                                    if (response.Status == ResponseStatus.Successful)
                                    {
                                        App.ShowProgress("rezervasyon başarıyla iptal edildi", ProgressType.Success, ProgressTime.Normal);
                                        Functions.UpdateTicket(Database.TempData.Ticket.PNR, NavigationService);
                                    }
                                    else
                                    {
                                        App.ShowProgress(response.Message, ProgressType.Error, ProgressTime.Normal);
                                    }
                                };
                                if (checkedCount != totalCount && checkedCount == 1)
                                    ReservationRequests.GetCancelReservation(Database.TempData.Ticket, checkedPassengers.First());
                                else
                                    ReservationRequests.GetCancelReservation(Database.TempData.Ticket);
                            }
                            else
                            {
                                Logger.Clicked("btnCancel");
                                btnContinue.IsActivated = true;
                                btnBack.IsActivated = true;
                            }
                        };
                    }
                }
            }
            else // if (Database.TempData.Ticket.ActionType == ActionType.Buying)
            {
                DefaultButtons.Visibility = Visibility.Visible;
                ContinueButtons.Visibility = Visibility.Collapsed;
                TicketPassengerItem.HideCheckBoxes();
                Header.HeaderConfirmed = (buttonId) =>
                {
                    if (buttonId == 0)
                    {
                        var callTask = new PhoneCallTask();
                        callTask.DisplayName = "Biletall Çağrı Merkezi";
                        callTask.PhoneNumber = "+908503603258";
                        callTask.Show();
                    }
                };
                App.ShowProgress("satış işlemleri bu sürümde iptal edilememektedir, Biletall Çağrı Merkezi'ni arayabilirsiniz", ProgressType.Plain, ProgressTime.Infinite, new List<string>() { "Ara", "Vazgeç" });
            }
        }

        private void btnContinue_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnContinue");
            int checkedCount = TicketPassengerItem.CheckedPassengerList.Count();

            if (checkedCount != 0)
            {
                App.RemoveBackPressedEvent(BackPressed);

                if (_actionType == 1)
                    BuyTicket();
                else if (_actionType == 2)
                    CancelTicket();
            }
            else
            {
                App.ShowProgress("devam edebilmek için en az 1 yolcu seçmelisiniz", ProgressType.Warning, ProgressTime.Normal);
            }
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnBack");
            App.RemoveBackPressedEvent(BackPressed);
            _actionType = -1;
            DefaultButtons.Visibility = Visibility.Visible;
            ContinueButtons.Visibility = Visibility.Collapsed;
            TicketPassengerItem.HideCheckBoxes();
        }

        private void BackPressed()
        {
            App.RemoveBackPressedEvent(BackPressed);
            _actionType = -1;
            DefaultButtons.Visibility = Visibility.Visible;
            ContinueButtons.Visibility = Visibility.Collapsed;
            TicketPassengerItem.HideCheckBoxes();
        }

        private void btnViewTicket_Clicked(object sender, EventArgs e)
        {
            Database.TempData.PNR = Database.TempData.Ticket.PNR;
            NavigationService.Navigate(new Uri("/Pages/TicketAction/ViewPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}