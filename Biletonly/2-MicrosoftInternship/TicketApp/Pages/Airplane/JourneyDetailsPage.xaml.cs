using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Biletall.Helper.Enums;
using Biletall.Helper.Requests;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls.Enums;
using TicketApp.Models;
using TicketApp.UserControls;

namespace TicketApp.Pages.Airplane
{
    public partial class JourneyDetailsPage : PhoneApplicationPage
    {
        public static Action OnClassesChecked = null;
        private static bool _isClassesChecked = false;

        public JourneyDetailsPage()
        {
            InitializeComponent();

            btnReserve.Tap += Button_Tap;
            btnBuy.Tap += Button_Tap;
        }

        void Button_Tap(object sender, GestureEventArgs e)
        {
            if (!_isClassesChecked)
                App.ShowProgress("sefer(ler) için sınıf seçmelisiniz", ProgressType.Warning, ProgressTime.Normal);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Uçuş Detayları");
            _isClassesChecked = false;

            FlightList.ItemsSource = Database.TempData.Ticket.Journeys.ToList();
            OnClassesChecked = () =>
            {
                _isClassesChecked = true;
                btnReserve.IsActivated = false;
                btnBuy.IsActivated = false;
                PriceDetailsGrid.Visibility = Visibility.Collapsed;

                if (!App.IsInternetAvailable)
                    App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
                else if (!JourneyClassesState())
                    App.ShowProgress("aktarma seferler için aynı sınıfı seçmelisiniz", ProgressType.Warning, ProgressTime.Normal);
                else
                    GetPriceDetails();
            };

            int i = 1;
            foreach (var journey in Database.TempData.Ticket.Journeys.ToList())
            {
                foreach (var segment in journey.Segments.ToList())
                {
                    DateTime departureDate = segment.DepartureDate;
                    if (departureDate.Hour <= 6)
                    {
                        string text = i + ". uçak " + departureDate.GetDescription() + " bağlayan gece kalkacaktır";
                        App.ShowProgress(text, ProgressType.Warning, ProgressTime.Normal);
                    }
                    i++;
                }
            }
        }

        private bool JourneyClassesState()
        {
            bool state = true;
            foreach (var journey in Database.TempData.Ticket.Journeys.ToList())
            {
                var selectedClass  = journey.Segments.First().SelectedClass;
                if (!journey.Segments.TrueForAll(x => x.SelectedClass.Name == selectedClass.Name))
                    state = false;
            }
            return state;
        }

        private void GetPriceDetails()
        {
            App.ShowProgress("fiyat detayları yükleniyor...");

            JourneyRequests.PriceDetailsRequest.OnCompleted = (response) =>
            {
                var priceDetail = response.Result;
                JourneyRequests.PriceDetailsRequest.OnCompleted = null;
                App.HideProgress();
                if (response.Status != ResponseStatus.Successful)
                {
                    if (response.Status == ResponseStatus.DifferentFactories)
                        App.ShowProgress("gidiş-dönüş biletinizi tek seferde alabilmeniz için seçtiğiniz firmalar aynı olmalıdır", ProgressType.Warning, ProgressTime.Infinite, new List<string>() { "Tamam, anladım" });
                    else
                        App.ShowProgress("fiyat detayları yüklenemedi", ProgressType.Error, ProgressTime.Normal);
                }
                else
                {
                    JourneyRequests.FillPassengerPrices(Database.TempData.Ticket, priceDetail);
                    Database.TempData.Ticket.Price = priceDetail.TotalPrice;

                    PriceDetailsGrid.DataContext = null;
                    PriceDetailsGrid.DataContext = priceDetail;
                    PriceDetailsGrid.Visibility = Visibility.Visible;

                    var grid = PassengersPricePanel.FindName("PassengersPrices") as Grid;
                    grid.Children.Clear();

                    var stackPanel = new StackPanel();
                    stackPanel.Children.Add(new FlightPriceRow());
                    stackPanel.Children.Add(new FlightPriceRow() { Price = priceDetail.AdultPrice, Label = "yetişkin", Visibility = priceDetail.AdultPrice.PassengerCount == 0 ? Visibility.Collapsed : Visibility.Visible });
                    stackPanel.Children.Add(new FlightPriceRow() { Price = priceDetail.ChildPrice, Label = "çocuk", Visibility = priceDetail.ChildPrice.PassengerCount == 0 ? Visibility.Collapsed : Visibility.Visible });
                    stackPanel.Children.Add(new FlightPriceRow() { Price = priceDetail.InfantPrice, Label = "yaşlı", Visibility = priceDetail.InfantPrice.PassengerCount == 0 ? Visibility.Collapsed : Visibility.Visible });
                    stackPanel.Children.Add(new FlightPriceRow() { Price = priceDetail.TeenagerPrice, Label = "genç", Visibility = priceDetail.TeenagerPrice.PassengerCount == 0 ? Visibility.Collapsed : Visibility.Visible });
                    stackPanel.Children.Add(new FlightPriceRow() { Price = priceDetail.StudentPrice, Label = "öğrenci", Visibility = priceDetail.StudentPrice.PassengerCount == 0 ? Visibility.Collapsed : Visibility.Visible });
                    stackPanel.Children.Add(new FlightPriceRow() { Price = priceDetail.BabyPrice, Label = "bebek", Visibility = priceDetail.BabyPrice.PassengerCount == 0 ? Visibility.Collapsed : Visibility.Visible });
                    stackPanel.Children.Add(new FlightPriceRow() { Price = priceDetail.MilitaryPrice, Label = "asker", Visibility = priceDetail.MilitaryPrice.PassengerCount == 0 ? Visibility.Collapsed : Visibility.Visible });
                    grid.Children.Add(stackPanel);

                    Functions.ScrollTo(Scroll, LayoutRoot, DetailsPanel.ActualHeight);
                    btnReserve.IsActivated = true;
                    btnBuy.IsActivated = true;
                }
            };
            JourneyRequests.GetPriceDetails(Database.TempData.Ticket);
        }

        private void btnReserve_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnReserve");
            Database.TempData.Ticket.ActionType = ActionType.Reservation;
            NavigationService.Navigate(new Uri("/Pages/Airplane/PassengerListPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnBuy_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnBuy");
            Database.TempData.Ticket.ActionType = ActionType.Buying;
            NavigationService.Navigate(new Uri("/Pages/Airplane/PassengerListPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}