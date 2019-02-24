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

namespace TicketApp
{
    public partial class PassengerInfoPage : PhoneApplicationPage
    {
        public PassengerInfoPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            List<string> list = new List<string>();
            list.Add("Deneme 1");
            list.Add("Deneme 2");
            list.Add("Deneme 3");
            FirstPassenger_Service.ItemsSource = list;
            SecondPassenger_Service.ItemsSource = list;
            ThirdPassenger_Service.ItemsSource = list;
            FourthPassenger_Service.ItemsSource = list;
            int yolcuSayisi = Database.SecilenKoltuklar.Count;
            if (yolcuSayisi < 4)
                FourthPassenger.Visibility = Visibility.Collapsed;
            if (yolcuSayisi < 3)
                ThirdPassenger.Visibility = Visibility.Collapsed;
            if (yolcuSayisi < 2)
                SecondPassenger.Visibility = Visibility.Collapsed;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            ProgressIndicator indicator = new ProgressIndicator();
            indicator.IsIndeterminate = true;
            indicator.IsVisible = true;
            indicator.Text = "İşlem gerçekleştiriliyor..";

            int yolcuSayisi = Database.SecilenKoltuklar.Count;
            Database.Yolcular = new List<Yolcu>();
            Database.Yolcular.Add(new Yolcu() { Adi = FirstPassenger_Name.Text, Soyadi = FirstPassenger_LastName.Text, BinisServisi = FirstPassenger_Service.SelectedItem.ToString(), Koltuk = Database.SecilenKoltuklar[0] });
            if (yolcuSayisi > 1)
                Database.Yolcular.Add(new Yolcu() { Adi = SecondPassenger_Name.Text, Soyadi = SecondPassenger_LastName.Text, BinisServisi = SecondPassenger_Service.SelectedItem.ToString(), Koltuk = Database.SecilenKoltuklar[1] });
            if (yolcuSayisi > 2)
                Database.Yolcular.Add(new Yolcu() { Adi = ThirdPassenger_Name.Text, Soyadi = ThirdPassenger_LastName.Text, BinisServisi = ThirdPassenger_Service.SelectedItem.ToString(), Koltuk = Database.SecilenKoltuklar[2] });
            if (yolcuSayisi > 3)
                Database.Yolcular.Add(new Yolcu() { Adi = FourthPassenger_Name.Text, Soyadi = FourthPassenger_LastName.Text, BinisServisi = FourthPassenger_Service.SelectedItem.ToString(), Koltuk = Database.SecilenKoltuklar[3] });
            Database.PhoneNumber = PhoneNumber.Text;
            Database.Email = Email.Text;

            string passengers = "";
            for (int i = 0; i < Database.Yolcular.Count; i++)
            {
                Yolcu yolcu = Database.Yolcular[i];
                passengers += "<KoltukNo" + (i + 1) + ">" + yolcu.Koltuk.No + "</KoltukNo" + (i + 1) + ">" +
                              "<Adi" + (i + 1) + ">" + yolcu.Adi + "</Adi" + (i + 1) + ">" +
                              "<Soyadi" + (i + 1) + ">" + yolcu.Soyadi + "</Soyadi" + (i + 1) + ">";
                              //"<ServisYeriKalkis" + (i + 1) + ">" + yolcu.BinisServisi + "</ServisYeriKalkis" + (i + 1) + ">";
            }
            string request = "<IslemRezervasyon>" +
                             "<FirmaNo>" + Database.SeferDetaylari.Firma.No + "</FirmaNo>" +
                             "<KalkisAdi>" + Database.SeferDetaylari.KalkisYeri + "</KalkisAdi>" +
                             "<VarisAdi>" + Database.SeferDetaylari.VarisYeri + "</VarisAdi>" +
                             "<Tarih>" + Database.SeferDetaylari.Tarih.ToString("yyyy-MM-dd") + "</Tarih>" +
                             "<Saat>" + Database.SeferDetaylari.Saat + "</Saat>" +
                             "<HatNo>" + Database.SeferDetaylari.HatNo + "</HatNo>" +
                             "<SeferNo>" + Database.SeferDetaylari.No + "</SeferNo>" +
                             passengers +
                             "<TelefonNo>" + Database.PhoneNumber + "</TelefonNo>" +
                             "<Cinsiyet>" + (Database.Cinsiyet == Cinsiyet.Bay ? "2" : "1") + "</Cinsiyet>" +
                             "<YolcuSayisi>" + yolcuSayisi + "</YolcuSayisi>" +
                             "<FirmaAciklama></FirmaAciklama>" +
                             "<HatirlaticiNot></HatirlaticiNot>" +
                             "<WebYolcu>" +
                             "<WebUyeNo>0</WebUyeNo>" +
                             "<Ip>127.0.0.1</Ip>" +
                             "<Email>" + Database.Email + "</Email>" +
                             "</WebYolcu>" +
                             "</IslemRezervasyon>";
            ServiceSoapClient client = new ServiceSoapClient();
            client.StrIsletAsync(request, Database.Admin);
            client.StrIsletCompleted += client_StrIsletCompleted;
            SystemTray.SetProgressIndicator(this, indicator);
        }
        void client_StrIsletCompleted(object sender, StrIsletCompletedEventArgs e)
        {
            string result = e.Result;
            Rezervasyon rezervasyon = Function.XmlToRezervasyon(result);
            if (rezervasyon == null)
            {
                MessageBox.Show("Bu sefer için rezervasyon yapılmamaktadır!");
            }
            else
            {
                if (rezervasyon.Sonuc == true)
                {
                    Database.Rezervasyonlar.Add(rezervasyon);
                    MessageBox.Show("Rezervasyon işleminiz başarıyla yapılmıştır.\nİşlemlere yönlendiriliyorsunuz..");
                    NavigationService.Navigate(new Uri("/MyActionsPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Rezervasyon işlemi gerçekleştirilemedi!");
                    NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
                }
            }
            if (SystemTray.ProgressIndicator != null)
                SystemTray.ProgressIndicator.IsVisible = false;
        }
    }
}