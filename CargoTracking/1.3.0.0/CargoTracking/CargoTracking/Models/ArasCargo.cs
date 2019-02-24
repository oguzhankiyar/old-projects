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
using HtmlAgilityPack;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CargoTracking.Models
{
    public class ArasCargo
    {
        public static Factory Factory = new Factory("Aras Kargo", "Assets/aras.png");
        private static string Details_URL { get; set; }
        private static string Movements_URL { get; set; }
        private static Tracking Tracking = new Tracking();


        public static void GetDetails()
        {
            string url = string.Format("http://appl-srv.araskargo.com.tr/CargoInfoV3.aspx?code={0}", Database.TrackingCode);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
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
                    GetURLs(webResult);

                    HttpWebRequest request1 = HttpWebRequest.Create(Details_URL) as HttpWebRequest;
                    request1.Method = "GET";
                    request1.BeginGetResponse(GetCallBack1, request1);
                }
                catch (WebException)
                {
                    Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                    Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                }
            }
            else
            {
                Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
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
                    GetTrackingDetails(webResult);

                    HttpWebRequest request2 = HttpWebRequest.Create(Movements_URL) as HttpWebRequest;
                    request2.Method = "GET";
                    request2.BeginGetResponse(GetCallBack2, request2);
                }
                catch (WebException)
                {
                    Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                    Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                }
            }
            else
            {
                Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
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
                    GetMovements(webResult);
                }
                catch (WebException)
                {
                    Database.Dispatcher.BeginInvoke(() =>
                    {
                        SystemTray.ProgressIndicator.IsVisible = false;
                        Database.WarningText.Visibility = Visibility.Visible;
                        Database.TrackingDetails.Visibility = Visibility.Collapsed;
                        Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                    });
                }
            }
            else
            {
                Database.Dispatcher.BeginInvoke(() =>
                {
                    SystemTray.ProgressIndicator.IsVisible = false;
                    Database.WarningText.Visibility = Visibility.Visible;
                    Database.TrackingDetails.Visibility = Visibility.Collapsed;
                    Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                });
            }
        }
        private static void GetTrackingDetails(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            Tracking.Code = Database.TrackingCode;
            Tracking.Factory = Factory;
            Tracking.OutputPlace = Function.TidyText(node.SelectSingleNode("//span[@id='LabelIlkCikis']//a").InnerText);
            Tracking.OutputDate = Function.TidyText(node.SelectSingleNode("//span[@id='cikis_tarihi']").InnerText);
            Tracking.InputPlace = Function.TidyText(node.SelectSingleNode("//span[@id='varis_subesi']//a").InnerText);
            Tracking.LastState = Function.TidyText(node.SelectSingleNode("//span[@id='Son_Durum']").InnerText);
            if (!string.IsNullOrEmpty(node.SelectSingleNode("//span[@id='Teslim_Alan']").InnerText))
                Tracking.Receiver = Function.TidyText(node.SelectSingleNode("//span[@id='Teslim_Alan']").InnerText) + " (" + node.SelectSingleNode("//span[@id='Teslim_Tarihi']").InnerText + ")";

        }
        private static void GetMovements(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            HtmlNodeCollection col = node.SelectNodes("//table[@id='transactionsDataGrid']//tr[position()>1]");
            foreach (HtmlNode item in col)
            {
                Movement hareket = new Movement();
                hareket.Location = item.SelectSingleNode("./td[3]").InnerText + " - " + item.SelectSingleNode("./td[2]").InnerText;
                hareket.Date = item.SelectSingleNode("./td[1]").InnerText;
                hareket.State = Function.TidyText(item.SelectSingleNode("./td[4]").InnerText);
                Tracking.Movements.Add(hareket);
            }
            Database.Dispatcher.BeginInvoke(() =>
            {
                SystemTray.ProgressIndicator.IsVisible = false;
                Tracking.LastSearchDate = DateTime.Now;
                Database.TrackingDetails.Visibility = Visibility.Visible;
                Database.WarningText.Visibility = Visibility.Collapsed;
                Database.TrackingDetails.DataContext = Tracking;
                Database.AddToHistory(Tracking);
            });
        }
        private static void GetURLs(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html.Replace("<li>", "").Replace("</li>", ""));
            HtmlNode node = doc.DocumentNode;
            if (node.InnerText.Contains("Kayıt Bulunamadı"))
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
                Details_URL = "http://appl-srv.araskargo.com.tr/" + node.SelectSingleNode("//div[@id='tabs']//a[1]").Attributes["href"].Value.Replace("&amp;", "&");
                Movements_URL = "http://appl-srv.araskargo.com.tr/" + node.SelectSingleNode("//div[@id='tabs']//a[2]").Attributes["href"].Value.Replace("&amp;", "&");
            }
        }
    }
}
