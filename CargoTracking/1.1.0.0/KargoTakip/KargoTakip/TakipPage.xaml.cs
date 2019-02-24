using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KargoTakip.Models;
using System.Windows.Media.Imaging;

namespace KargoTakip
{
    public partial class TakipPage : PhoneApplicationPage
    {

        public TakipPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Function.GecmisiYukle();
            if (Data._Sirket == null)
                SirketButton.Source = new BitmapImage(new Uri("/Assets/ApplicationIcon.png", UriKind.Relative));
            else
                SirketButton.Source = new BitmapImage(new Uri(Data._Sirket.Resim, UriKind.Relative));
            if (Data._TakipKodu != null)
                TakipKodu.Text = Data._TakipKodu;
            if (Data._TakipDetay != null)
            {
                TakipDetay.Visibility = Visibility.Visible;
                TakipDetay.DataContext = Data._TakipDetay;
            }
            if (NavigationContext.QueryString.ContainsKey("SirketAdi") && NavigationContext.QueryString.ContainsKey("Kod"))
            {
                string SirketAdi = NavigationContext.QueryString["SirketAdi"];
                Sirket sirket = Data.Sirketler.SingleOrDefault(x => x.Adi == SirketAdi);
                string Kod = NavigationContext.QueryString["Kod"];
                SirketButton.Source = new BitmapImage(new Uri(sirket.Resim, UriKind.Relative));
                Data._Sirket = sirket;
                Data._TakipKodu = Kod;
                TakipKodu.Text = Kod;
                GirisYap();
            }
            base.OnNavigatedTo(e);
        }
        private void GirisYap()
        {
            try
            {
                ProgressIndicator indicator = new ProgressIndicator();
                indicator.IsIndeterminate = true;
                indicator.IsVisible = true;
                indicator.Text = "Bilgiler yükleniyor..";
                SystemTray.SetProgressIndicator(this, indicator);
                switch (Data._Sirket.Adi)
                {
                    case "Aras Kargo":
                        Dispatcher.BeginInvoke(() => ArasKargo.TakipGetir(Dispatcher, TakipDetay, Data._TakipKodu));
                        break;
                    case "DHL Kargo":
                        Dispatcher.BeginInvoke(() => DHLKargo.TakipGetir(Dispatcher, TakipDetay, Data._TakipKodu));
                        break;
                    case "MNG Kargo":
                        Dispatcher.BeginInvoke(() => MNGKargo.TakipGetir(Dispatcher, TakipDetay, Data._TakipKodu));
                        break;
                    case "PTT Kargo":
                        Dispatcher.BeginInvoke(() => PTTKargo.TakipGetir(Dispatcher, TakipDetay, Data._TakipKodu));
                        break;
                    case "Sürat Kargo":
                        Dispatcher.BeginInvoke(() => SuratKargo.TakipGetir(Dispatcher, TakipDetay, Data._TakipKodu));
                        break;
                    case "UPS Kargo":
                        Dispatcher.BeginInvoke(() => UPSKargo.TakipGetir(Dispatcher, TakipDetay, Data._TakipKodu));
                        break;
                    case "Yurtiçi Kargo":
                        Dispatcher.BeginInvoke(() => YurticiKargo.TakipGetir(Dispatcher, TakipDetay, Data._TakipKodu));
                        break;
                    default:
                        break;
                }
                Sirketler.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                MessageBox.Show("Bir sorun oluştu!\nTekrar deneyiniz..");
            }
        }
        private void GirisButton_Click(object sender, RoutedEventArgs e)
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
            {
                MessageBox.Show("İnternet bağlantınızı kontrol ediniz!");
            }
            else if (Data._Sirket == null)
            {
                MessageBox.Show("Kargo şirketini seçmediniz!\nSoldaki resme tıklayarak seçiminizi yapınız.");
            }
            else if(string.IsNullOrEmpty(TakipKodu.Text))
            {
                MessageBox.Show("Takip kodunuz yazmadınız!");
            }
            else
            {
                GirisYap();
            }
        }

        private void SirketButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Sirketler.Visibility == Visibility.Collapsed)
            {
                Sirketler.DataContext = Data.Sirketler;
                Sirketler.Visibility = Visibility.Visible;
                TakipDetay.Visibility = Visibility.Collapsed;
            }
            else
            {
                Sirketler.Visibility = Visibility.Collapsed;
                TakipDetay.Visibility = TakipDetay.DataContext != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void SirketlerLLS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sirket sirket = SirketlerLLS.SelectedItem as Sirket;
            Data._Sirket = sirket;
            Sirketler.Visibility = Visibility.Collapsed;
            SirketButton.Source = new BitmapImage(new Uri("" + sirket.Resim, UriKind.RelativeOrAbsolute));
        }

        private void InfoImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HakkindaPage.xaml", UriKind.Relative));
        }

        private void TakipKodu_TextChanged(object sender, TextChangedEventArgs e)
        {
            Data._TakipKodu = TakipKodu.Text;
        }
    }
}