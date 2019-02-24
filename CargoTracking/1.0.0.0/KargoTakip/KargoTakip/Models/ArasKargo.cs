using HtmlAgilityPack;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KargoTakip.Models
{
    class ArasKargo
    {
        private static string Kod = null;
        private static Grid TakipDetay;
        private static Dispatcher _dispatcher;
        private static Takip Detaylar = new Takip();
        public static Sirket Sirket = new Sirket() { Adi = "Aras Kargo", Resim = "/Assets/aras.png" };

        private static string GenelBilgiler_URL { get; set; }
        private static string Hareketler_URL { get; set; }

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
            kod = kod.ToUpper();
            Kod = kod;
            string url = string.Format("http://appl-srv.araskargo.com.tr/CargoInfoV3.aspx?code={0}", kod);
            TakipDetay = grid;
            _dispatcher = dispatcher;
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.BeginGetResponse(GetCallBack, request);
        }
        private static Takip TakipDetayGetir(string html)
        {
            throw new NotImplementedException();
        }
        private static void URLGetir(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.Replace("<li>", "").Replace("</li>", ""));
            HtmlNode node = doc.DocumentNode;
            if (node.InnerText.Contains("Kayıt Bulunamadı"))
            {
                _dispatcher.BeginInvoke(() =>
                {
                    TakipDetay.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Bu koda ait kayıt bulunamadı!\nTekrar deneyiniz..");
                    TakipDetay.DataContext = Data._TakipDetay = null;
                });
            }
            else
            {
                GenelBilgiler_URL = "http://appl-srv.araskargo.com.tr/" + node.SelectSingleNode("//div[@id='tabs']//a[1]").Attributes["href"].Value.Replace("&amp;", "&");
                Hareketler_URL = "http://appl-srv.araskargo.com.tr/" + node.SelectSingleNode("//div[@id='tabs']//a[2]").Attributes["href"].Value.Replace("&amp;", "&");
            }
        }
        private static void GetCallBack(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            if (webRequest != null)
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(result);
                    StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string webResult = stream.ReadToEnd();
                    URLGetir(webResult);

                    HttpWebRequest request1 = HttpWebRequest.Create(GenelBilgiler_URL) as HttpWebRequest;
                    request1.Method = "GET";
                    request1.BeginGetResponse(GetCallBack1, request1);
                }
                catch (WebException)
                {
                    _dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                    _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                }
            }
            else
            {
                _dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
            }
        }
        private static void GetCallBack1(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            if (webRequest != null)
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(result);
                    StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string webResult = stream.ReadToEnd();
                    GenelBilgileriGetir(webResult);

                    HttpWebRequest request2 = HttpWebRequest.Create(Hareketler_URL) as HttpWebRequest;
                    request2.Method = "GET";
                    request2.BeginGetResponse(GetCallBack2, request2);
                }
                catch (WebException)
                {
                    _dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                    _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                }
            }
            else
            {
                _dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
            }
        }
        private static void GetCallBack2(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            if (webRequest != null)
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(result);
                    StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string webResult = stream.ReadToEnd();
                    HareketleriGetir(webResult);
                }
                catch (WebException)
                {
                    _dispatcher.BeginInvoke(() =>
                    {
                        SystemTray.ProgressIndicator.IsVisible = false;
                        TakipDetay.Visibility = Visibility.Collapsed;
                        _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                    });
                }
            }
            else
            {
                _dispatcher.BeginInvoke(() =>
                {
                    SystemTray.ProgressIndicator.IsVisible = false;
                    TakipDetay.Visibility = Visibility.Collapsed;
                    _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                });
            }
        }
        private static void GenelBilgileriGetir(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            Detaylar.Kod = Kod;
            Detaylar.Sirket = Sirket;
            Detaylar.CikisBirimi = Function.TidyText(node.SelectSingleNode("//span[@id='LabelIlkCikis']//a").InnerText);
            Detaylar.CikisTarihi = Function.TidyText(node.SelectSingleNode("//span[@id='cikis_tarihi']").InnerText);
            Detaylar.VarisBirimi = Function.TidyText(node.SelectSingleNode("//span[@id='varis_subesi']//a").InnerText);
            Detaylar.SonDurum = Function.TidyText(node.SelectSingleNode("//span[@id='Son_Durum']").InnerText);
            if (!string.IsNullOrEmpty(node.SelectSingleNode("//span[@id='Teslim_Alan']").InnerText))
                Detaylar.Teslim = Function.TidyText(node.SelectSingleNode("//span[@id='Teslim_Alan']").InnerText) + " (" + node.SelectSingleNode("//span[@id='Teslim_Tarihi']").InnerText + ")";
            
        }
        private static void HareketleriGetir(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            HtmlNodeCollection col = node.SelectNodes("//table[@id='transactionsDataGrid']//tr[position()>1]");
            List<Hareket> hareketler = new List<Hareket>();
            foreach (HtmlNode item in col)
            {
                Hareket hareket = new Hareket();
                hareket.Birim = item.SelectSingleNode("./td[3]").InnerText + " - " + item.SelectSingleNode("./td[2]").InnerText;
                hareket.Tarih = item.SelectSingleNode("./td[1]").InnerText;
                hareket.Islem = Function.TidyText(item.SelectSingleNode("./td[4]").InnerText);
                hareketler.Add(hareket);
            }
            Detaylar.Hareketler = hareketler;
            _dispatcher.BeginInvoke(() =>
            {
                SystemTray.ProgressIndicator.IsVisible = false;
                Detaylar.SonGiris = DateTime.Now;
                TakipDetay.Visibility = Visibility.Visible;
                TakipDetay.DataContext = Detaylar;
                Function.GecmiseEkle(Detaylar);
            });
        }
    }
}
