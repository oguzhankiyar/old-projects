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
    public class YurticiCargo
    {
        public static Factory Factory = new Factory("Yurtiçi Kargo", "Assets/yurtici.png");
        private static byte[] byteArray;
        private static Tracking Tracking = new Tracking();
        public static void GetDetails()
        {
            string url = string.Format("http://selfservis.yurticikargo.com/reports/SSWDocumentDetail.aspx?DocId={0}", Database.TrackingCode);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.BeginGetResponse(GetCallBack0, request);
        }

        private static void GetCallBack0(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            if (webRequest != null)
            {
                try
                {
                    WebResponse response = webRequest.EndGetResponse(result);
                    Encoding iso = new ISO88599Encoding();
                    StreamReader stream = new StreamReader(response.GetResponseStream(), iso);
                    string webResult = stream.ReadToEnd();
                    GetTrackingDetails(webResult);

                    HttpWebRequest request = HttpWebRequest.Create("http://selfservis.yurticikargo.com/ajaxpro/SelfService.Web.reports.SSWDocumentDetail,SelfService.Web.ashx") as HttpWebRequest;
                    request.Method = "POST";
                    request.Headers["X-Ajaxpro-Method"] = "GetCargoHistory";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.UserAgent = "runscope/0.1";
                    string postData = "{\"docId\":\"" + Database.TrackingCode + "\"}";
                    byteArray = Encoding.UTF8.GetBytes(postData);
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
                if (node.InnerText.Contains("Kayıt Bulunamadı"))
                    throw new Exception();
                Tracking.Code = Database.TrackingCode;
                Tracking.Factory = Factory;
                HtmlNodeCollection nodes = node.SelectNodes("//tr");
                string teslimTarihi = "";

                for (int i = 0; i < nodes.Count; i++)
                {
                    HtmlNode item = nodes[i];
                    string text = item.SelectSingleNode(".//td[2]").InnerText;
                    if (i == 5)
                        Tracking.OutputPlace = Function.TidyText(text);
                    else if (i == 4)
                        Tracking.InputPlace = Function.TidyText(text);
                    else if (i == 0)
                        Tracking.OutputDate = text;
                    else if (i == 16)
                        teslimTarihi = text;
                    else if (i == 17)
                        Tracking.Receiver = Function.TidyText(text) + (string.IsNullOrEmpty(teslimTarihi) ? null : " (" + teslimTarihi + ")");
                    else if (i == 14)
                        Tracking.LastState = Function.TidyText(text);
                }
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

        public static void GetMovements(string result)
        {
            try
            {
                Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                Database.Dispatcher.BeginInvoke(() => SystemTray.ProgressIndicator.IsVisible = false);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result);
                HtmlNode node = doc.DocumentNode;
                HtmlNodeCollection col = node.SelectNodes("//table//tr");
                for (int i = 1; i < col.Count; i++)
                {
                    HtmlNode item = col[i];
                    Movement movement = new Movement();
                    movement.Date = item.SelectSingleNode("./td[2]").InnerText + " " + item.SelectSingleNode("./td[3]").InnerText;
                    movement.State = Function.TidyText(item.SelectSingleNode("./td[4]").InnerText);
                    movement.Location =
                        Function.TidyText(item.SelectSingleNode("./td[6]").InnerText + " - " +
                                          item.SelectSingleNode("./td[7]").InnerText + " " +
                                          item.SelectSingleNode("./td[8]").InnerText);
                    Tracking.Movements.Insert(0, movement);
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
