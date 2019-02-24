using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages
{
    public partial class Buying3DSecurePage : PhoneApplicationPage
    {
        public Buying3DSecurePage()
        {
            InitializeComponent();

            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                App.AddBackPressedEvent(BackPressed);
                App.ShowProgress("onay sayfası yükleniyor...");
                BuyingRequests.OnPageLoaded = () =>
                {
                    App.HideProgress();
                };

                BuyingRequests.On3DSecureBuyingCompleted = (response) =>
                {
                    var actionResult = response.Result;
                    App.RemoveBackPressedEvent(BackPressed);
                    if (response.Status == ResponseStatus.Successful)
                    {
                        Database.AddPassenger(Database.TempData.Ticket.Passengers.ToList());

                        App.ShowProgress("biletiniz başarıyla alınmıştır, yönlendiriliyorsunuz...", ProgressType.Success, ProgressTime.Normal);
                        Database.TempData.Ticket.PNR.Code = actionResult.PNR;
                        Database.TempData.Ticket.PNR.Parameter = Database.TempData.Ticket.Passengers[0].LastName;
                        Database.TempData.Ticket.PNR.ExpirationDate = actionResult.ExpirationDate;
                        Database.SavedData.Tickets.Add(Database.TempData.Ticket);
                        Database.FillSavedPNR();
                        var timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromMilliseconds(2000);
                        timer.Tick += (x, r) =>
                        {
                            timer.Stop();
                            NavigationService.Navigate(new Uri("/Pages/TicketAction/DetailsPage.xaml", UriKind.RelativeOrAbsolute));
                            App.ClearBackHistory();
                        };
                        timer.Start();
                    }
                    else if (response.Status == ResponseStatus.InvalidTCKN)
                    {
                        App.ShowProgress("belirttiğiniz yolcuların bilgilerini kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
                        if (NavigationService.CanGoBack)
                            NavigationService.GoBack();
                    }
                    else
                    {
                        App.ShowProgress(response.Message, ProgressType.Plain, ProgressTime.Infinite, new List<string>() { "Yeniden düzenle", "Vazgeç" });
                        Header.HeaderConfirmed = (buttonId) =>
                        {
                            if (buttonId == 0)
                            {
                                if (NavigationService.CanGoBack)
                                    NavigationService.GoBack();
                            }
                            else
                            {
                                NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
                                App.ClearBackHistory();
                            }
                        };
                    }
                };
                BuyingRequests.Get3DSecureBuying(Database.TempData.Ticket, this.webBrowserGrid);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Ödeme Onayı");
        }

        private void BackPressed()
        {
            return;
        }
    }
}