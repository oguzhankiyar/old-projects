using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.BiletAllService;
using TicketApp.Models;
using System.Windows.Media;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TicketApp
{
    public partial class SelectSeatPage : PhoneApplicationPage
    {
        private int passengerCount;
        public SelectSeatPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = "Koltuklar yükleniyor..";
            ServiceSoapClient client = new ServiceSoapClient();
            string query = "<Otobus>" +
                           "<FirmaNo>" + Database.SeferDetaylari.Firma.No + "</FirmaNo>" +
                           "<KalkisAdi>" + Database.SeferDetaylari.KalkisYeri + "</KalkisAdi>" +
                           "<VarisAdi>" + Database.SeferDetaylari.VarisYeri + "</VarisAdi>" +
                           "<Tarih>" + Database.SeferDetaylari.Tarih.ToString("yyyy-MM-dd") + "</Tarih>" +
                           "<Saat>" + Database.SeferDetaylari.Saat + "</Saat>" +
                           "<HatNo>" + Database.SeferDetaylari.HatNo + "</HatNo>" +
                           "<IslemTipi>" + Database.SeferDetaylari.IslemTipi + "</IslemTipi>" +
                           "<SeferTakipNo>" + Database.SeferDetaylari.TakipNo + "</SeferTakipNo>" +
                           "</Otobus>";
            client.StrIsletAsync(query, Database.Admin);
            client.StrIsletCompleted += client_StrIsletCompleted;
            SystemTray.SetProgressIndicator(this, indicator);
            passengerCount = Convert.ToInt32(NavigationContext.QueryString["passengerCount"]);
            MaleGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
            MaleText.Foreground = new SolidColorBrush(Colors.White);
            Database.Cinsiyet = Cinsiyet.Bay;
            base.OnNavigatedTo(e);
        }

        void client_StrIsletCompleted(object sender, StrIsletCompletedEventArgs e)
        {
            string result = e.Result;
            Database.YolcuSayisi = passengerCount;
            Database.Koltuklar = Function.XmlToKoltuk(result);
            Function.CreateSeats(SeatsGrid);
            if (SystemTray.ProgressIndicator != null)
                SystemTray.ProgressIndicator.IsVisible = false;
        }

        private void Male_Tap(object sender, GestureEventArgs e)
        {
            MaleGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
            MaleText.Foreground = new SolidColorBrush(Colors.White);
            FamaleGrid.Background = new SolidColorBrush(Colors.White);
            FamaleText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x69, 0xB4));
            Database.Cinsiyet = Cinsiyet.Bay;
        }

        private void Famale_Tap(object sender, GestureEventArgs e)
        {
            FamaleGrid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x69, 0xB4));
            FamaleText.Foreground = new SolidColorBrush(Colors.White);
            MaleGrid.Background = new SolidColorBrush(Colors.White);
            MaleText.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0xFF));
            Database.Cinsiyet = Cinsiyet.Bayan;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (Database.YolcuSayisi != 0)
                MessageBox.Show("Tüm yolcuların koltuklarını seçmediniz!");
            else
                NavigationService.Navigate(new Uri("/PassengerInfoPage.xaml", UriKind.Relative));
        }
    }
}