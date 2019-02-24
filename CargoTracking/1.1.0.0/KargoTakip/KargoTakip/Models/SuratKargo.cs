using HtmlAgilityPack;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KargoTakip.Models
{
    [DataContract]
    class SuratKargo
    {
        [DataMember]
        private static string Kod = null;
        [DataMember]
        private static Grid TakipDetay;
        [DataMember]
        private static Dispatcher _dispatcher;
        [DataMember]
        public static Sirket Sirket = new Sirket() { Adi = "Sürat Kargo", Resim = "/Assets/surat.png" };

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
            kod = kod.ToUpper();
            Kod = kod;
            string url = string.Format("http://suratkargo.com.tr/?p=kargom_nerede_post&TakipNo={0}", kod);
            TakipDetay = grid;
            _dispatcher = dispatcher;
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.BeginGetResponse(GetCallBack, request);
        }
        public static Takip TakipDetayGetir(string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode node = doc.DocumentNode;
                if (html.Contains("Kayıt Bulunamadı"))
                    return null;
                Takip takip = new Takip();
                takip.Kod = Kod;
                takip.Sirket = Sirket;
                takip.CikisBirimi = Function.TidyText(node.SelectSingleNode("//div[@class='online_bg_orta']//div[2]//div[13]").InnerText);
                takip.CikisTarihi = node.SelectSingleNode("//tr[2]//td[2]").InnerHtml.Replace("<br>", " ");
                takip.VarisBirimi = Function.TidyText(node.SelectSingleNode("//div[@class='online_bg_orta']//div[3]//div[13]").InnerText);
                if (!string.IsNullOrEmpty(node.SelectSingleNode("//tr[2]//td[6]").InnerText))
                    takip.Teslim = Function.TidyText(node.SelectSingleNode("//tr[2]//td[6]").InnerText) + " (" + node.SelectSingleNode("//tr[2]//td[4]").InnerHtml.Replace("<br>", " ") +")";
                takip.SonDurum = Function.TidyText(node.SelectSingleNode("//tr[2]//td[7]").InnerText);
                Hareket hareket = new Hareket();
                hareket.Tarih = node.SelectSingleNode("//tr[2]//td[4]").InnerText;
                hareket.Birim = Function.TidyText(node.SelectSingleNode("//tr[2]//td[9]").InnerText);
                hareket.Islem = Function.TidyText(node.SelectSingleNode("//tr[2]//td[8]").InnerText);
                List<Hareket> hareketler = new List<Hareket>();
                hareketler.Add(hareket);
                takip.Hareketler = hareketler;
                return takip;
            }
            catch (Exception)
            {
                return null;
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
                    Takip detay = TakipDetayGetir(webResult);
                    _dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                    if (detay == null)
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
                        _dispatcher.BeginInvoke(() =>
                        {
                            detay.SonGiris = DateTime.Now;
                            Function.GecmiseEkle(detay);
                            TakipDetay.Visibility = Visibility.Visible;
                            TakipDetay.DataContext = Data._TakipDetay = detay;
                        });
                    }
                }
                catch (WebException)
                {
                    _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                }
            }
            else
            {
                _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
            }
        }
    }
}
