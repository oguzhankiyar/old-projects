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
    public class SuratCargo
    {
        public static Factory Factory = new Factory("Sürat Kargo", "Assets/surat.png");

        public static void GetDetails()
        {
            string url = string.Format("http://suratkargo.com.tr/?p=kargom_nerede_post&TakipNo={0}", Database.TrackingCode);
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
                if (result.Contains("Kayıt Bulunamadı"))
                    return null;
                Tracking tracking = new Tracking();
                tracking.Code = Database.TrackingCode;
                tracking.Factory = Factory;
                tracking.OutputPlace = Function.TidyText(node.SelectSingleNode("//div[@class='online_bg_orta']//div[2]//div[13]").InnerText);
                tracking.OutputDate = node.SelectSingleNode("//tr[2]//td[2]").InnerHtml.Replace("<br>", " ");
                tracking.InputPlace = Function.TidyText(node.SelectSingleNode("//div[@class='online_bg_orta']//div[3]//div[13]").InnerText);
                if (!string.IsNullOrEmpty(node.SelectSingleNode("//tr[2]//td[6]").InnerText))
                    tracking.Receiver = Function.TidyText(node.SelectSingleNode("//tr[2]//td[6]").InnerText) + " (" + node.SelectSingleNode("//tr[2]//td[4]").InnerHtml.Replace("<br>", " ") + ")";
                tracking.LastState = Function.TidyText(node.SelectSingleNode("//tr[2]//td[7]").InnerText);
                Movement hareket = new Movement();
                hareket.Date = node.SelectSingleNode("//tr[2]//td[4]").InnerText;
                hareket.Location = Function.TidyText(node.SelectSingleNode("//tr[2]//td[9]").InnerText);
                hareket.State = Function.TidyText(node.SelectSingleNode("//tr[2]//td[8]").InnerText);
                tracking.Movements.Add(hareket);
                return tracking;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
