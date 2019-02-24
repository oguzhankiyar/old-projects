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
    public class DhlCargo
    {
        public static Factory Factory = new Factory("DHL Kargo", "Assets/dhl.png");

        public static void GetDetails()
        {
            string url = string.Format("http://www.dhl.com.tr/content/tr/tr/express/tracking.shtml?brand=DHL&AWB={0}", Database.TrackingCode);
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
                if (node.InnerText.Contains("Talebiniz için herhangi bir sonuç bulunamadı"))
                    return null;
                Tracking tracking = new Tracking();
                tracking.Code = Database.TrackingCode;
                tracking.Factory = Factory;
                doc.LoadHtml(node.SelectSingleNode("//div[@id='table" + tracking.Code + "']").InnerHtml);
                node = doc.DocumentNode;
                tracking.OutputPlace = Function.TidyText(node.SelectSingleNode("//thead[@class='tophead']//th[3]//span[1]").InnerText);
                tracking.InputPlace = Function.TidyText(node.SelectSingleNode("//thead[@class='tophead']//th[3]//span[2]").InnerText);
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
                        Movement movement = new Movement();
                        movement.Location = item.SelectSingleNode("./td[3]").InnerText.Trim();
                        movement.State = item.SelectSingleNode("./td[2]").InnerText.Trim();
                        movement.Date = currentDate + " " + item.SelectSingleNode("./td[4]").InnerText.Replace("at", ", ").Replace("  ", " ").Trim();
                        tracking.Movements.Insert(0, movement);
                        if (j == 1)
                        {
                            tracking.LastState = movement.State;
                            if (tracking.LastState.Contains("teslim edildi"))
                            {
                                string teslim = " - ";
                                try
                                {
                                    teslim = Function.TidyText(tracking.LastState.Split(':')[1].Trim());
                                    teslim += " (" + movement.Date + ")";
                                }
                                catch (Exception) { }
                                tracking.Receiver = teslim;
                            }
                        }
                    }
                }
                tracking.OutputDate = currentDate;
                return tracking;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
