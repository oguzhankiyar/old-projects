using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Models;
using TicketApp.BiletAllService;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TicketApp
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = "Şehirler yükleniyor..";
            ServiceSoapClient client = new ServiceSoapClient();
            client.StrIsletAsync("<Kalkis><FirmaNo>0</FirmaNo><IlceleriGoster>1</IlceleriGoster></Kalkis>", Database.Admin);
            client.StrIsletCompleted += client_StrIsletCompleted;
            SystemTray.SetProgressIndicator(this, indicator);
            if (string.IsNullOrEmpty(DateFrom.Text))
                DateFrom.Text = DateTime.Now.ToString("d MMMMMMM yyyy");
            if (string.IsNullOrEmpty(PassengerCount.Text))
                PassengerCount.Text = "1";
            if (string.IsNullOrEmpty(ActionType.Text))
                ActionType.Text = "Rezervasyon";
            base.OnNavigatedTo(e);
        }

        private void client_StrIsletCompleted(object sender, StrIsletCompletedEventArgs e)
        {
            string result = e.Result;
            List<Sehir> sehirler = Function.XmlToSehir(result);
            if (WhereFromCities.ItemsSource == null)
                WhereFromCities.ItemsSource = sehirler;
            if (WhereToCities.ItemsSource == null)
                WhereToCities.ItemsSource = sehirler;
            if (Database.Nereden != null && WhereFromCities.SelectedItem != null)
                WhereFromCities.SelectedIndex = sehirler.FindIndex(x => x.Adi == Database.Nereden);
            if (Database.Nereye != null && WhereToCities.SelectedItem != null)
                WhereToCities.SelectedIndex = sehirler.FindIndex(x => x.Adi == Database.Nereye);
            if (SystemTray.ProgressIndicator != null)
                SystemTray.ProgressIndicator.IsVisible = false;
        }

        private void WhereFromCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sehir selected = WhereFromCities.SelectedItem as Sehir;
            if (selected != null)
            {
                WhereFromCity.Text = selected.Adi;
                Database.Nereden = selected.Adi;
            }
        }
        private void WhereToCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sehir selected = WhereToCities.SelectedItem as Sehir;
            if (selected != null)
            {
                WhereToCity.Text = selected.Adi;
                Database.Nereye = selected.Adi;
            }
        }

        private void DateFromPicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            DateFrom.Text = ((DateTime)DateFromPicker.Value).ToString("d MMMMMMM yyyy");
            Database.Tarih = ((DateTime)DateFromPicker.Value);
        }

        private void PassengerUp_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int count = Convert.ToInt32(PassengerCount.Text);
            if (count != 4)
                PassengerCount.Text = count + 1 + "";
        }

        private void PassengerDown_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int count = Convert.ToInt32(PassengerCount.Text);
            if (count != 0)
                PassengerCount.Text = count - 1 + "";
        }

        private void ActionType_Tap(object sender, GestureEventArgs e)
        {
            ActionType.Text = ActionType.Text == "Satış" ? "Rezervasyon" : "Satış";
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (PassengerCount.Text == "0")
            {
                MessageBox.Show("Yolcu sayısını girmeniz gerekiyor!");
            }
            else
            {
                NavigationService.Navigate(new Uri("/BusSearchPage.xaml?whereFrom=" + WhereFromCity.Text + "&whereTo=" + WhereToCity.Text + "&dateFrom=" + ((DateTime)DateFromPicker.Value).ToString("yyyy-MM-dd") + "&passengerCount=" + PassengerCount.Text + "&actionType=" + ActionType.Text, UriKind.Relative));
            }
        }
    }
}