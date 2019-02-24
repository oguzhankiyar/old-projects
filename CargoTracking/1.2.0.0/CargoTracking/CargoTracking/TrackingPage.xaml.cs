using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using CargoTracking.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CargoTracking
{
    public partial class TrackingPage : PhoneApplicationPage
    {
        private Factory _selectedFactory = null;

        public TrackingPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Factories.ItemsSource == null)
                Factories.ItemsSource = Database.Factories.ToList();

            Database.Dispatcher = Dispatcher;
            Database.TrackingDetails = TrackingDetails;
            Database.WarningText = WarningText;

            if (TrackingDetails.DataContext == null)
            {
                TrackingDetails.Visibility = Visibility.Collapsed;
                Factories.Visibility = Visibility.Visible;
            }
            else
            {
                TrackingDetails.Visibility = Visibility.Visible;
                Factories.Visibility = Visibility.Collapsed;
            }
            WarningText.Visibility = Visibility.Collapsed;

            if (NavigationContext.QueryString.ContainsKey("trackingCode") &&
                NavigationContext.QueryString.ContainsKey("factoryName"))
            {
                Factories.SelectedItem =
                    Database.Factories.SingleOrDefault(x => x.Name == NavigationContext.QueryString["factoryName"]);
                TxtTrackingCode.Text = NavigationContext.QueryString["trackingCode"];
                Factories.Visibility = Visibility.Collapsed;
                Search();
            }

            base.OnNavigatedTo(e);
        }

        private void Search()
        {
            ProgressIndicator indicator = new ProgressIndicator
            {
                IsIndeterminate = true,
                IsVisible = true,
                Text = "Bilgiler yükleniyor.."
            };
            SystemTray.SetProgressIndicator(this, indicator);

            Database.TrackingCode = TxtTrackingCode.Text.ToUpper();
            Database.SelectedFactory = _selectedFactory;
            switch (_selectedFactory.Name)
            {
                case "Aras Kargo":
                    ArasCargo.GetDetails();
                    break;
                case "DHL Kargo":
                    DhlCargo.GetDetails();
                    break;
                case "MNG Kargo":
                    MngCargo.GetDetails();
                    break;
                case "PTT Kargo":
                    PttCargo.GetDetails();
                    break;
                case "Sürat Kargo":
                    SuratCargo.GetDetails();
                    break;
                case "UPS Kargo":
                    UpsCargo.GetDetails();
                    break;
                case "Yurtiçi Kargo":
                    YurticiCargo.GetDetails();
                    break;
            }
        }

        private void Next_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HistoryPage.xaml", UriKind.Relative));
        }

        private void SelectFactory_Tap(object sender, GestureEventArgs e)
        {
            if (Factories.Visibility == Visibility.Collapsed)
            {
                TrackingDetails.Visibility = Visibility.Collapsed;
                WarningText.Visibility = Visibility.Collapsed;
                Factories.Visibility = Visibility.Visible;
            }
            else
            {
                Factories.Visibility = Visibility.Collapsed;
                if (TrackingDetails.DataContext != null)
                {
                    WarningText.Visibility = Visibility.Collapsed;
                    TrackingDetails.Visibility = Visibility.Visible;
                }
                else
                {
                    TrackingDetails.Visibility = Visibility.Collapsed;
                    WarningText.Visibility = Visibility.Visible;
                }
            }
        }

        private void Factories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Factories.SelectedItem as Factory;
            SelectedFactory.Source = new BitmapImage(new Uri(selected == null ? "Assets/ApplicationIcon.png" : selected.ImageSource, UriKind.Relative));
            _selectedFactory = selected;
            Factories.Visibility = Visibility.Collapsed;
            TrackingDetails.Visibility = Visibility.Collapsed;
            WarningText.Visibility = Visibility.Visible;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                MessageBox.Show("İnternet bağlantınızı kontrol ediniz!");
            else if (_selectedFactory == null)
                MessageBox.Show("Kargo şirketini seçmediniz!");
            else if (string.IsNullOrEmpty(TxtTrackingCode.Text))
                MessageBox.Show("Takip kodunu yazmadınız!");
            else
                Search();
        }
    }
}