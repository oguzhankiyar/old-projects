using HtmlAgilityPack;
using Microsoft.Phone.Shell;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace KargoTakip.Models
{
    class PTTKargo
    {
        private static string Kod = null;
        private static Grid TakipDetay;
        private static Dispatcher _dispatcher;
        public static Sirket Sirket = new Sirket() { Adi = "PTT Kargo", Resim = "/Assets/ptt.png" };

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
            kod = kod.ToUpper();
            Kod = kod;
            string url = string.Format("https://wap.ptt.gov.tr/cepptt/android/posta/yurtDisiKargoSorgula?barkod={0}", kod);
            TakipDetay = grid;
            _dispatcher = dispatcher;
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.BeginGetResponse(GetCallBack, request);
        }
        public static Takip TakipDetayGetir(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                if (obj["sonucAciklama"].ToString().Contains("Barkod Numarasna Ait İslem Bilgisi Bulunamadı."))
                    return null;
                Takip takip = new Takip();
                takip.Kod = Kod;
                takip.Sirket = Sirket;
                takip.Teslim = Function.TidyText(obj["teslim_alan"].ToString()); 
                takip.CikisBirimi = " - ";
                takip.VarisBirimi = " - ";
                takip.CikisTarihi = " - ";
                try
                {
                    JArray array = obj["dongu"] as JArray;
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject obj2 = array[i] as JObject;
                        takip.CikisBirimi = array.Count > 0 ? Function.TidyText(array[0]["islemYeri"].ToString()) : " - ";
                        takip.VarisBirimi = !string.IsNullOrEmpty(obj["teslim_alan"].ToString()) && array.Count > 0 ? Function.TidyText(array[array.Count - 1]["islemYeri"].ToString()) : " - ";
                        takip.CikisTarihi = array.Count > 0 ? array[0]["tarih"].ToString() : " - ";
                        takip.SonDurum = array.Count > 0 ? array[array.Count - 1]["yapilanIslem"].ToString() : null;
                        Hareket hareket = new Hareket();
                        try
                        {
                            hareket.Birim = Function.TidyText(obj2["ofis"].ToString()) + " - " + Function.TidyText(obj2["islemYeri"].ToString());
                        }
                        catch { }
                        hareket.Tarih = obj2["tarih"].ToString();
                        hareket.Islem = obj2["yapilanIslem"].ToString();
                        takip.Hareketler.Add(hareket);
                    }
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
