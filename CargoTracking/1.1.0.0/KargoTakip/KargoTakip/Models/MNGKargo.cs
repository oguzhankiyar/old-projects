using HtmlAgilityPack;
using Microsoft.Phone.Shell;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KargoTakip.Models
{
    class MNGKargo
    {
        private static string Kod = null;
        private static Grid TakipDetay;
        private static Dispatcher _dispatcher;
        public static Sirket Sirket = new Sirket() { Adi = "MNG Kargo", Resim = "/Assets/mng.png" };

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
            kod = kod.ToUpper();
            Kod = kod;
            string url = string.Format("http://service.mngkargo.com.tr/iactive/takip.asp?fatseri={0}&fatnumara={1}", kod.Substring(0, 2), kod.Substring(2));
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
                if (node.InnerText.Contains("Aradığınız Alım Belgesi Numarasına Ait Kayıt Bulunamadı."))
                    return null;
                Takip takip = new Takip();
                takip.Kod = Kod;
                takip.Sirket = Sirket;
                takip.CikisBirimi = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-fatura-birim']").InnerText);
                takip.VarisBirimi = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-teslim-birim']").InnerText);
                takip.CikisTarihi = node.SelectSingleNode("//div[@class='tkp-fatura-tarih']").InnerText;
                string teslimTarihi = "";
                if (node.SelectSingleNode("//div[@class='tkp-teslim-tarih']").InnerText.Trim() != "-")
                    teslimTarihi = node.SelectSingleNode("//div[@class='tkp-teslim-tarih']").InnerText + " " + node.SelectSingleNode("//div[@class='tkp-teslim-saat']").InnerText;
                takip.Teslim = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-teslim-alan']").InnerText) + (string.IsNullOrEmpty(teslimTarihi) ? null : " (" + teslimTarihi + ")");

                try
                {
                    Hareket hareket = new Hareket();
                    if (node.SelectSingleNode("//div[@class='tkp-teslim-tarih']").InnerText.Trim() != "-")
                    {
                        hareket.Birim = takip.VarisBirimi;
                        hareket.Tarih = teslimTarihi;
                        hareket.Islem = "Teslim edildi.";
                        takip.Hareketler.Add(hareket);
                    }
                    takip.SonDurum = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-hareket-aciklama']").InnerText);
                    hareket = new Hareket();
                    if (node.SelectSingleNode("//div[@class='tkp-hareket-tarih']").InnerText.Trim() != "-")
                        hareket.Tarih = node.SelectSingleNode("//div[@class='tkp-hareket-tarih']").InnerText.Trim() + " - " + node.SelectSingleNode("//div[@class='tkp-hareket-saat']").InnerText.Trim();
                    else
                        hareket.Tarih = " - ";
                    if (!string.IsNullOrEmpty(node.SelectSingleNode("//div[contains(@class, 'tkp-hareket-tur')]").InnerText))
                        hareket.Islem = Function.TidyText(node.SelectSingleNode("//div[contains(@class, 'tkp-hareket-tur')]").InnerText);
                    takip.Hareketler.Add(hareket);
                }
                catch (Exception) { }
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
