using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using HtmlAgilityPack;
using Microsoft.Phone.Shell;

namespace CargoTracking.Models
{
    public class MngCargo
    {
        public static Factory Factory = new Factory("MNG Kargo", "Assets/mng.png");

        public static void GetDetails()
        {
            string url = string.Format("http://service.mngkargo.com.tr/iactive/takip.asp?fatseri={0}&fatnumara={1}", Database.TrackingCode.Substring(0, 2), Database.TrackingCode.Substring(2));
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
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
                    Tracking tracking = GetTracking(webResult);
                    Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                    if (tracking == null)
                    {
                        Database.Dispatcher.BeginInvoke(() =>
                            {
                                Database.TrackingDetails.Visibility = Visibility.Collapsed;
                                Database.WarningText.Visibility = Visibility.Visible;
                                MessageBox.Show("Bu koda ait kayıt bulunamadı!\nTekrar deneyiniz..");
                                Database.TrackingDetails.DataContext = null;
                            });
                    }
                    else
                    {
                        Database.Dispatcher.BeginInvoke(() => 
                            {
                                tracking.LastSearchDate = DateTime.Now;
                                Database.AddToHistory(tracking);
                                Database.TrackingDetails.Visibility = Visibility.Visible;
                                Database.WarningText.Visibility = Visibility.Collapsed;
                                Database.TrackingDetails.DataContext = tracking;
                            });
                    }
                }
                catch (WebException)
                {
                    Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                }
            }
            else
            {
                Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
            }
        }
        

        public static Tracking GetTracking(string result)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);
                HtmlNode node = doc.DocumentNode;
                if (node.InnerText.Contains("Aradığınız Alım Belgesi Numarasına Ait Kayıt Bulunamadı."))
                    return null;
                Tracking takip = new Tracking();
                takip.Code = Database.TrackingCode;
                takip.Factory = Factory;
                takip.OutputPlace = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-fatura-birim']").InnerText);
                takip.InputPlace = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-teslim-birim']").InnerText);
                takip.OutputDate = node.SelectSingleNode("//div[@class='tkp-fatura-tarih']").InnerText;
                string teslimTarihi = "";
                if (node.SelectSingleNode("//div[@class='tkp-teslim-tarih']").InnerText.Trim() != "-")
                    teslimTarihi = node.SelectSingleNode("//div[@class='tkp-teslim-tarih']").InnerText + " " + node.SelectSingleNode("//div[@class='tkp-teslim-saat']").InnerText;
                takip.Receiver = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-teslim-alan']").InnerText) + (string.IsNullOrEmpty(teslimTarihi) ? null : " (" + teslimTarihi + ")");

                try
                {
                    Movement hareket = new Movement();
                    if (node.SelectSingleNode("//div[@class='tkp-teslim-tarih']").InnerText.Trim() != "-")
                    {
                        hareket.Location = takip.InputPlace;
                        hareket.Date = teslimTarihi;
                        hareket.State = "Teslim edildi.";
                        takip.Movements.Add(hareket);
                    }
                    takip.LastState = Function.TidyText(node.SelectSingleNode("//div[@class='tkp-hareket-aciklama']").InnerText);
                    hareket = new Movement();
                    if (node.SelectSingleNode("//div[@class='tkp-hareket-tarih']").InnerText.Trim() != "-")
                        hareket.Date = node.SelectSingleNode("//div[@class='tkp-hareket-tarih']").InnerText.Trim() + " - " + node.SelectSingleNode("//div[@class='tkp-hareket-saat']").InnerText.Trim();
                    else
                        hareket.Date = null;
                    if (!string.IsNullOrEmpty(node.SelectSingleNode("//div[contains(@class, 'tkp-hareket-tur')]").InnerText))
                        hareket.State = Function.TidyText(node.SelectSingleNode("//div[contains(@class, 'tkp-hareket-tur')]").InnerText);
                    takip.Movements.Add(hareket);
                }
                catch (Exception) { }
                return takip;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
