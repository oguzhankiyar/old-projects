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
    public class UpsCargo
    {
        public static Factory Factory = new Factory("UPS Kargo", "Assets/ups.png");
        private static CookieContainer myCookieContainer = new CookieContainer();
        private static byte[] byteArray;
        private static Tracking Tracking = new Tracking();

        public static void GetDetails()
        {
            string url = string.Format("http://ups.com.tr/WaybillSorgu.aspx?Waybill={0}", Database.TrackingCode);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.CookieContainer = myCookieContainer;
            request.BeginGetResponse(GetCallBack0, request);
        }

        private static void GetCallBack0(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            if (webRequest != null)
            {
                try
                {
                    HttpWebResponse response = webRequest.EndGetResponse(result) as HttpWebResponse;
                    StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string webResult = stream.ReadToEnd();
                    GetTrackingDetails(webResult);

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(webResult);
                    HtmlNode node = doc.DocumentNode;

                    string viewState = node.SelectSingleNode("//input[@id='__VIEWSTATE']").Attributes["value"].Value;
                    string eventTarget = "ctl00$MainContent$LinkButtonSonIslemGoster";
                    string eventValidation = node.SelectSingleNode("//input[@id='__EVENTVALIDATION']").Attributes["value"].Value;
                    string eventArgument = "";

                    string postData =
                        string.Format(
                            "__VIEWSTATE={0}&__EVENTVALIDATION={1}&__EVENTTARGET={2}&__EVENTARGUMENT={3}&loc={4}&results={5}&view={6}",
                            HttpUtility.UrlEncode(viewState),
                            HttpUtility.UrlEncode(eventValidation),
                            HttpUtility.UrlEncode(eventTarget),
                            HttpUtility.UrlEncode(eventArgument),
                            HttpUtility.UrlEncode("tr_TR"),
                            HttpUtility.UrlEncode("25"),
                            HttpUtility.UrlEncode("both"));
                    byteArray = Encoding.UTF8.GetBytes(postData);

                    /*Hareketler için tekrar bağlantı (Viewstate ile)*/
                    string url = string.Format("http://ups.com.tr/WaybillSorgu.aspx?Waybill={0}", Database.TrackingCode);
                    HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    request.Headers["X-MicrosoftAjax"] = "Delta=true";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.131 Safari/537.36";
                    request.CookieContainer = myCookieContainer;
                    request.ContentLength = byteArray.Length;

                    request.BeginGetRequestStream(GetCallBack1, request);
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

        private static void GetCallBack1(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            if (webRequest != null)
            {
                Stream dataStream = webRequest.EndGetRequestStream(result);
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                webRequest.BeginGetResponse(GetCallBack2, webRequest);
            }
            else
            {
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
                    Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
                }
            }
            else
            {
                Database.Dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
            }
        }
        public static void GetTrackingDetails(string result)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);
                HtmlNode node = doc.DocumentNode;
                Tracking.Code = Database.TrackingCode;
                Tracking.Factory = Factory;
                Tracking.InputPlace = Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelAliciIl']").InnerText) + " - " + Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelAliciIlce']").InnerText);
                Tracking.LastState = Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_Label3']").InnerText);
                if (!string.IsNullOrEmpty(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelTeslimAlan']").InnerText))
                    Tracking.Receiver = Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelTeslimAlan']").InnerText) + " (" + node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelTeslimTarihi']").InnerText + ")";
            }
            catch (Exception)
            {
                Database.Dispatcher.BeginInvoke(() =>
                {
                    Database.TrackingDetails.Visibility = Visibility.Collapsed;
                    Database.WarningText.Visibility = Visibility.Visible;
                    Database.TrackingDetails.DataContext = null;
                });
            }
        }

        private static void GetMovements(string result)
        {
            try
            {
                Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);
                HtmlNode node = doc.DocumentNode;
                HtmlNodeCollection col = node.SelectNodes("//table[@id='ctl00_MainContent_DataListSonIslem']//table");
                for (int i = 1; i < col.Count; i++)
                {
                    HtmlNode item = col[i];
                    Movement movement = new Movement();
                    movement.Date = item.SelectSingleNode("./tr//td[1]").InnerText;
                    movement.State = Function.TidyText(item.SelectSingleNode("./tr//td[2]").InnerText);
                    movement.Location = Function.TidyText(item.SelectSingleNode("./tr//td[3]").InnerText);
                    Tracking.Movements.Add(movement);
                    if (i == 1)
                    {
                        Tracking.OutputDate = movement.Date.ToString();
                        Tracking.OutputPlace = movement.Location;
                    }
                }
                Database.Dispatcher.BeginInvoke(() =>
                {
                    Tracking.LastSearchDate = DateTime.Now;
                    Database.AddToHistory(Tracking);
                    Database.TrackingDetails.Visibility = Visibility.Visible;
                    Database.WarningText.Visibility = Visibility.Collapsed;
                    Database.TrackingDetails.DataContext = Tracking;
                });
            }
            catch (Exception)
            {
                Database.Dispatcher.BeginInvoke(() =>
                {
                    Database.TrackingDetails.Visibility = Visibility.Collapsed;
                    Database.WarningText.Visibility = Visibility.Visible;
                    MessageBox.Show("Bu koda ait kayıt bulunamadı!\nTekrar deneyiniz..");
                    Database.TrackingDetails.DataContext = null;
                });
            }
        }

    }
}
