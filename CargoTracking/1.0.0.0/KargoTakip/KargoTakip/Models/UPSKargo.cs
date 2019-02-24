using HtmlAgilityPack;
using Microsoft.Phone.Shell;
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

namespace KargoTakip.Models
{
    class UPSKargo
    {
        private static string Kod = null;
        private static Grid TakipDetay;
        private static Dispatcher _dispatcher;
        public static Sirket Sirket = new Sirket() { Adi = "UPS Kargo", Resim = "/Assets/ups.png" };
        private static CookieContainer myCookieContainer = new CookieContainer();
        private static byte[] byteArray;

        public static void TakipGetir(Dispatcher dispatcher, Grid grid, string kod)
        {
            kod = kod.ToUpper();
            Kod = kod;
            string url = string.Format("http://ups.com.tr/WaybillSorgu.aspx?Waybill={0}", kod);
            TakipDetay = grid;
            _dispatcher = dispatcher;
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.CookieContainer = myCookieContainer;
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

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(webResult);
                    HtmlNode node = doc.DocumentNode;

                    string viewState = node.SelectSingleNode("//input[@id='__VIEWSTATE']").Attributes["value"].Value;
                    string eventTarget = "ctl00$MainContent$LinkButtonSonIslemGoster";
                    string eventValidation = node.SelectSingleNode("//input[@id='__EVENTVALIDATION']").Attributes["value"].Value;
                    string eventArgument = "";
                    string link = "ctl00$MainContent$UpdatePanelSonIslem|ctl00$MainContent$LinkButtonSonIslemGoster";

                    string postData = string.Format("__VIEWSTATE={0}&__EVENTVALIDATION={1}&__EVENTTARGET={2}&__EVENTARGUMENT={3}&ctl00$MainContent$ScriptManager1={4}&loc={5}&results={6}&view={7}",
                        viewState,
                        eventValidation,
                        eventTarget,
                        eventArgument,
                        link,
                        "tr_TR",
                        "25",
                        "both");
                    byteArray = Encoding.UTF8.GetBytes(postData) ;

                    /*Hareketler için tekrar bağlantı (Viewstate ile)*/
                    string url = string.Format("http://ups.com.tr/WaybillSorgu.aspx?Waybill={0}", Kod);
                    HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                    request.Method = "POST";

                    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    request.Headers["X-MicrosoftAjax"] = "Delta=true";
                    //request.Headers["Origin"] = "http://ups.com.tr";
                    //equest.Headers[HttpRequestHeader.Referer] = url;

                    request.CookieContainer = myCookieContainer;
                    request.ContentLength = byteArray.Length;
                    request.AllowAutoRedirect = true;
                    request.BeginGetRequestStream(GetCallBack0, request);
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
        private static void GetCallBack0(IAsyncResult result)
        {
            HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
            if (webRequest != null)
            {
                Stream stream = webRequest.EndGetRequestStream(result);
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();
                webRequest.BeginGetResponse(GetCallBack1, webRequest);
            }
            else
            {
                _dispatcher.BeginInvoke(() => MessageBox.Show("Sunucuya bağlanılamıyor..\nTekrar deneyiniz.."));
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
                Takip takip = new Takip();
                takip.Kod = Kod;
                takip.Sirket = Sirket;
                //takip.CikisBirimi - hareketlerin ilk işlemi
                //takip.CikisTarihi - hareketleri ilk işlemi
                takip.VarisBirimi = Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelAliciIl']").InnerText) + " - " + Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelAliciIlce']").InnerText);
                takip.SonDurum = Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_Label3']").InnerText);
                if (!string.IsNullOrEmpty(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelTeslimAlan']").InnerText))
                    takip.Teslim = Function.TidyText(node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelTeslimAlan']").InnerText) + " (" + node.SelectSingleNode("//span[@id='ctl00_MainContent_LabelTeslimTarihi']").InnerText + ")";
                return takip;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
