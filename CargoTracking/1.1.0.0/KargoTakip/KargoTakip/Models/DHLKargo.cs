using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using HtmlAgilityPack;
using System.IO;
using System.Windows;
using Microsoft.Phone.Shell;

namespace KargoTakip.Models
{
    class DHLKargo
    {
        private static string Kod = null;
        private static Grid TakipDetay;
        private static Dispatcher _dispatcher;
        public static Sirket Sirket = new Sirket() { Adi = "DHL Kargo", Resim = "/Assets/dhl.png" };

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
            kod = kod.ToUpper();
            Kod = kod;
            _dispatcher = dispatcher;
            TakipDetay = grid;
            string url = string.Format("http://www.dhl.com.tr/content/tr/tr/express/tracking.shtml?brand=DHL&AWB={0}", Kod);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.BeginGetResponse(GetCallBack, request);
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

        private static Takip TakipDetayGetir(string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode node = doc.DocumentNode;
                if (node.InnerText.Contains("Talebiniz için herhangi bir sonuç bulunamadı"))
                    return null;
                Takip takip = new Takip();
                takip.Kod = Kod;
                takip.Sirket = Sirket;
                doc.LoadHtml(node.SelectSingleNode("//div[@id='table" + Kod + "']").InnerHtml);
                node = doc.DocumentNode;
                takip.CikisBirimi = Function.TidyText(node.SelectSingleNode("//thead[@class='tophead']//th[3]//span[1]").InnerText);
                takip.VarisBirimi = Function.TidyText(node.SelectSingleNode("//thead[@class='tophead']//th[3]//span[2]").InnerText);
                takip.Hareketler = new List<Hareket>();
                HtmlNodeCollection col = node.SelectNodes("//tr");
                string currentDate = "";
                int j = 0;
                for (int i = 2; i < col.Count; i++)
                {
                    HtmlNode item = col[i];
                    if (item.ParentNode.Name == "thead" && !string.IsNullOrEmpty(item.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "")))
                    {
                        currentDate = item.SelectSingleNode("./th[1]").InnerText.Trim();
                    }
                    else if (item.ParentNode.Name == "tbody")
                    {
                        j++;
                        Hareket hareket = new Hareket();
                        hareket.Birim = item.SelectSingleNode("./td[3]").InnerText.Trim();
                        hareket.Islem = item.SelectSingleNode("./td[2]").InnerText.Trim();
                        hareket.Tarih = currentDate + " " + item.SelectSingleNode("./td[4]").InnerText.Replace("at", ", ").Replace("  ", " ").Trim();
                        takip.Hareketler.Insert(0, hareket);
                        if (j == 1)
                        {
                            takip.SonDurum = hareket.Islem;
                            if (takip.SonDurum.Contains("teslim edildi"))
                            {
                                string teslim = " - ";
                                try
                                {
                                    teslim = Function.TidyText(takip.SonDurum.Split(':')[1].Trim());
                                    teslim += " (" + hareket.Tarih + ")";
                                }
                                catch (Exception) { }
                                takip.Teslim = teslim;
                            }
                        }
                    }
			    }
                takip.CikisTarihi = currentDate;
                return takip;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
