using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;
using Biletall.Helper.Enums;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using TicketApp.Models;

namespace TicketApp.Pages.TicketAction
{
    public partial class CancelBuyingPage : PhoneApplicationPage
    {
        public CancelBuyingPage()
        {
            InitializeComponent();
        }

        private void btnSendCode_Clicked(object sender, EventArgs e)
        {
            var request = HttpWebRequest.Create("http://localhost:2230/api");//Biletall.Helper.ApiHelper.ApiUrl);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.BeginGetRequestStream(SendDataCallback, request);
        }

        private void SendDataCallback(IAsyncResult result)
        {
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                var obj = new JObject();
                obj.Add("version", Constraints.ApiVersion);
                obj.Add("username", Constraints.ApiUsername);
                obj.Add("password", Constraints.ApiPassword);
                var codeObj = new JObject();
                codeObj.Add("email", "ogzkyr@windowslive.com");//Database.TempData.Ticket.PNR.Email);
                var requestObj = new JObject();
                requestObj.Add("type", Biletall.Helper.ApiAction.GetVerificationCode);
                requestObj.Add("verification", codeObj);
                obj.Add("request", requestObj);

                string json = obj.ToString();
                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                using (var stream = request.EndGetRequestStream(result))
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Close();
                }
                request.BeginGetResponse(GetResponseCallBack, request);
            }
        }

        private void GetResponseCallBack(IAsyncResult result)
        {
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                var response = request.EndGetResponse(result);
                var reader = new StreamReader(response.GetResponseStream());
                var jsonResult = reader.ReadToEnd();

                var root = JObject.Parse(jsonResult);

                Dispatcher.BeginInvoke(() =>
                {
                    if (Convert.ToBoolean(root["status"].ToString()))
                        App.ShowProgress("onay kodu gönderildi", Controls.Enums.ProgressType.Success, Controls.Enums.ProgressTime.Normal);
                    else
                        App.ShowProgress("bir sorun oluştu, daha sonra tekrar deneyiniz", Controls.Enums.ProgressType.Error, Controls.Enums.ProgressTime.Normal);
                });
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            var request = HttpWebRequest.Create("http://localhost:2230/api");//Biletall.Helper.ApiHelper.ApiUrl);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.BeginGetRequestStream(SendDataCallback_2, request);
        }

        private string GetCancel()
        {
            var sb = new StringBuilder();
            sb.Append("<PnrIslem>");
            sb.Append("<PnrNo>3GS237</PnrNo>");
            if (Database.TempData.Ticket.Type == TicketType.BusJourney)
            {
                string seats = "";
                foreach (var item in Database.TempData.Ticket.Passengers)
                {
                    seats += item.Seat.Number + ",";
                }
                seats.TrimEnd(',');
                sb.Append("<PnrKoltukNo>" + seats + "</PnrKoltukNo>");
            }
            sb.Append("<WebUyeNo>0</WebUyeNo>");
            sb.Append("<PnrIslemTip>8</PnrIslemTip>");
            sb.Append("<PnrSatisIptalTutar>" + Database.TempData.Ticket.Price.TotalPrice.ToString("0.0000") + "</PnrSatisIptalTutar>");
            sb.Append("</PnrIslem>");

            return sb.ToString();
        }

        private void SendDataCallback_2(IAsyncResult result)
        {
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                var obj = new JObject();
                obj.Add("version", Constraints.ApiVersion);
                obj.Add("username", Constraints.ApiUsername);
                obj.Add("password", Constraints.ApiPassword);
                var cancelObj = new JObject();
                cancelObj.Add("pnr", Database.TempData.Ticket.PNR.Code);
                string seats = "";
                foreach (var item in Database.TempData.Ticket.Passengers)
                {
                    seats += item.Seat.Number + ",";
                }
                seats.TrimEnd(',');
                cancelObj.Add("seats", seats);
                cancelObj.Add("price", Database.TempData.Ticket.Price.TotalPrice.ToString("0.0000"));
                var requestObj = new JObject();
                requestObj.Add("type", Biletall.Helper.ApiAction.CancelBuying);
                requestObj.Add("code", txtCode.Value.ToString());
                requestObj.Add("ticket", cancelObj);
                obj.Add("request", requestObj);

                string json = obj.ToString();
                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                using (var stream = request.EndGetRequestStream(result))
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Close();
                }
                request.BeginGetResponse(GetResponseCallBack_2, request);
            }
        }
        private void GetResponseCallBack_2(IAsyncResult result)
        {
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                var response = request.EndGetResponse(result);
                var reader = new StreamReader(response.GetResponseStream());
                var jsonResult = reader.ReadToEnd();

                var root = JObject.Parse(jsonResult);

                Dispatcher.BeginInvoke(() =>
                {
                    if (Convert.ToBoolean(root["status"].ToString()))
                    {
                        txtInfo.Text = root["result"].ToString();
                        var element = XElement.Parse(root["result"].ToString());
                        bool isSuccessful = element.Element(XName.Get("Sonuc")).Value.ToLower() == "true";

                        if (isSuccessful && element.Element(XName.Get("Tutar")) != null)
                        {
                            txtInfo.Text = "biletiniz başarıyla iptal edilmiştir, sonucunda " + element.Element(XName.Get("Tutar")).Value.Replace("-", "") + " Biletall açık para oluşmuştur. Bu PNR kodu ile uygulamamızda veya http://biletall.com.tr sitesinde kullanabilirsiniz.";
                        }
                        else if (isSuccessful)
                        {
                            txtInfo.Text = "biletiniz başarıyla iptal edilmiştir";
                        }
                        else
                        {
                            if (element.Element(XName.Get("Hata")) != null)
                                txtInfo.Text = element.Element(XName.Get("Hata")).Value;
                            else
                                txtInfo.Text = "biletiniz iptal edilemedi";
                        }
                        App.ShowProgress("onay kodu gönderildi", Controls.Enums.ProgressType.Success, Controls.Enums.ProgressTime.Normal);
                    }
                    else
                        txtInfo.Text = "bir sorun oluştu, daha sonra tekrar deneyiniz";
                });
            }
        }
    }
}