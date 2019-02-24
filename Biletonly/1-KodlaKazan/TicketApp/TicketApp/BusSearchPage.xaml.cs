using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Linq;
using Windows.Phone.Media.Capture;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.BiletAllService;
using TicketApp.Models;
using Color = System.Windows.Media.Color;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TicketApp
{
    public partial class BusSearchPage : PhoneApplicationPage
    {
        private string dateFrom;
        private string actionType;
        private int orderType;
        private string passengerCount;
        private List<Sefer> list; 
        public BusSearchPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = "Seferler yükleniyor..";
            string whereFrom = NavigationContext.QueryString["whereFrom"];
            string whereTo = NavigationContext.QueryString["whereTo"];
            dateFrom = NavigationContext.QueryString["dateFrom"];
            passengerCount = NavigationContext.QueryString["passengerCount"];
            actionType = NavigationContext.QueryString["actionType"] == "Satış" ? "0" : "1";
            string requestFrom = "<Sefer>" +
                                 "<FirmaNo>0</FirmaNo>" +
                                 "<KalkisAdi>" + whereFrom + "</KalkisAdi>" +
                                 "<VarisAdi>" + whereTo + "</VarisAdi>" +
                                 "<Tarih>" + dateFrom + "</Tarih>" +
                                 "<AraNoktaGelsin>1</AraNoktaGelsin>" +
                                 "<IslemTipi>" + actionType + "</IslemTipi>" +
                                 "<YolcuSayisi>" + passengerCount + "</YolcuSayisi>" +
                                 "</Sefer>";
            ServiceSoapClient client = new ServiceSoapClient();
            client.StrIsletAsync(requestFrom, Database.Admin);
            client.StrIsletCompleted += client_StrIsletCompleted;
            SystemTray.SetProgressIndicator(this, indicator);
            WhereFrom.Text = whereFrom;
            WhereTo.Text = whereTo;
            TimeGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
            TimeText.Foreground = new SolidColorBrush(Colors.White);
            TimeImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.Relative));
            orderType = 3;
            base.OnNavigatedTo(e);
        }


        private void client_StrIsletCompleted(object sender, StrIsletCompletedEventArgs e)
        {
            string result = e.Result;
            if (result.Contains("Aradığınız Kriterlere Uygun Sefer Bulunamamıştır!"))
            {
                MessageBox.Show("Aradığınız kriterlere uygun sefer bulunamamıştır!");
                NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            }
            else
            {
                ControlsGrid.Visibility = Visibility.Visible;
                Seferler_LLS.ItemsSource = list = Function.XmlToSefer(result).OrderBy(x => x.Saat).ToList();
            }
            if (SystemTray.ProgressIndicator != null)
                SystemTray.ProgressIndicator.IsVisible = false;
        }

        private void Price_Tap(object sender, GestureEventArgs e)
        {
            if (orderType == 1)
            {
                TimeGrid.Background = new SolidColorBrush(Colors.White);
                TimeText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                PriceGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                PriceText.Foreground = new SolidColorBrush(Colors.White);
                PriceImage.Source = new BitmapImage(new Uri("/Assets/up.png", UriKind.Relative));
                orderType = 2;
                Seferler_LLS.ItemsSource = list.OrderByDescending(x => x.Fiyat).ToList();
            }
            else
            {
                TimeGrid.Background = new SolidColorBrush(Colors.White);
                TimeText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                PriceGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                PriceText.Foreground = new SolidColorBrush(Colors.White);
                TimeImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.Relative));
                PriceImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.Relative));
                orderType = 1;
                Seferler_LLS.ItemsSource = list.OrderBy(x => x.Fiyat).ToList();
            }
        }

        private void Time_Tap(object sender, GestureEventArgs e)
        {
            if (orderType == 3)
            {
                PriceGrid.Background = new SolidColorBrush(Colors.White);
                PriceText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                TimeGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                TimeText.Foreground = new SolidColorBrush(Colors.White);
                TimeImage.Source = new BitmapImage(new Uri("/Assets/up.png", UriKind.Relative));
                orderType = 4;
                Seferler_LLS.ItemsSource = list.OrderByDescending(x => x.HareketSaati).ToList();
            }
            else
            {
                PriceGrid.Background = new SolidColorBrush(Colors.White);
                PriceText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                TimeGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
                TimeText.Foreground = new SolidColorBrush(Colors.White);
                PriceImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.Relative));
                TimeImage.Source = new BitmapImage(new Uri("/Assets/down.png", UriKind.Relative));
                orderType = 3;
                Seferler_LLS.ItemsSource = list.OrderBy(x => x.HareketSaati).ToList();
            }
        }

        private void Seferler_LLS_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Seferler_LLS.SelectedItem != null)
            {
                Sefer selected = Seferler_LLS.SelectedItem as Sefer;
                selected.Tarih = Convert.ToDateTime(dateFrom);
                selected.IslemTipi = Convert.ToInt32(actionType);
                Database.SeferDetaylari = selected;
                NavigationService.Navigate(new Uri("/SelectSeatPage.xaml?passengerCount=" + passengerCount, UriKind.Relative));
            }
        }

    }
}