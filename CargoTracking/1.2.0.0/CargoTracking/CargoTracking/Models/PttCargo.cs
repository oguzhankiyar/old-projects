using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;

namespace CargoTracking.Models
{
    public class PttCargo
    {
        public static Factory Factory = new Factory("PTT Kargo", "Assets/ptt.png");

        public static void GetDetails()
        {
            string url = string.Format("https://wap.ptt.gov.tr/cepptt/android/posta/yurtDisiKargoSorgula?barkod={0}", Database.TrackingCode);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.BeginGetResponse(GetCallBack, request);
        }

        private static void GetCallBack(IAsyncResult result)
        {
            try
            {
                HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
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

        public static Tracking GetTracking(string result)
        {
            try
            {
                JObject obj = JObject.Parse(result);
                if (obj["sonucAciklama"].ToString().Contains("Barkod Numarasna Ait İslem Bilgisi Bulunamadı."))
                    return null;
                Tracking tracking = new Tracking();
                tracking.Code = Database.TrackingCode;
                tracking.Factory = Factory;
                tracking.Receiver = Function.TidyText(obj["teslim_alan"].ToString());
                tracking.OutputPlace = " - ";
                tracking.InputPlace = " - ";
                tracking.OutputDate = null;
                try
                {
                    JArray array = obj["dongu"] as JArray;
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject obj2 = array[i] as JObject;
                        tracking.OutputPlace = Function.TidyText(array[0]["islemYeri"].ToString());
                        tracking.InputPlace = !string.IsNullOrEmpty(obj["teslim_alan"].ToString()) ? Function.TidyText(array[array.Count - 1]["islemYeri"].ToString()) : " - ";
                        tracking.OutputDate = array[0]["tarih"].ToString();
                        tracking.LastState = array[array.Count - 1]["yapilanIslem"].ToString();
                        Movement movement = new Movement();
                        try
                        {
                            movement.Location = Function.TidyText(obj2["ofis"].ToString()) + " - " + Function.TidyText(obj2["islemYeri"].ToString());
                        }
                        catch { }
                        movement.Date = obj2["tarih"].ToString();
                        movement.State = obj2["yapilanIslem"].ToString();
                        tracking.Movements.Add(movement);
                    }
                }
                catch (Exception) { }
                return tracking;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
