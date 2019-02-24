using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls.Enums;
using TicketApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Biletall.Helper;

namespace TicketApp.Pages.Help
{
    public partial class FeedbackPage : PhoneApplicationPage
    {
        private byte[] _byteArray;

        public FeedbackPage()
        {
            InitializeComponent();
            App.SetTitle("Geri Bildirim");
        }

        private void Send_Clicked(object sender, EventArgs e)
        {
            if (!App.IsInternetAvailable)
                App.ShowProgress("internet bağlantınızı kontrol ediniz", ProgressType.Warning, ProgressTime.Normal);
            else if (cbBug.IsChecked == false && cbSuggestion.IsChecked == false && cbOther.IsChecked == false)
                App.ShowProgress("geri bildirim türünü seçmelisiniz", ProgressType.Warning, ProgressTime.Normal);
            else if (string.IsNullOrEmpty(txtName.Value) || string.IsNullOrEmpty(txtSubject.Value)|| string.IsNullOrEmpty(txtMessage.Value))
                App.ShowProgress("boş bıraktığınız alan(lar) var", ProgressType.Warning, ProgressTime.Normal);
            else if (!Regex.Match(txtEmail.Value.ToString(), @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$").Success)
                App.ShowProgress("email adresiniz geçerli değil", ProgressType.Warning, ProgressTime.Normal);
            else if (txtMessage.Value.Length < 20)
                App.ShowProgress("mesajınız en az 20 karakter olmalıdır", ProgressType.Warning, ProgressTime.Normal);
            else
            {
                App.ShowProgress("geri bildiriminiz gönderiliyor...");
                btnSend.IsActivated = false;

                var rootObj = new JObject();
                rootObj.Add("version", Constraints.ApiVersion);
                rootObj.Add("username", Constraints.ApiUsername);
                rootObj.Add("password", Constraints.ApiPassword);
                var requestObj = new JObject();
                requestObj.Add("type", ApiAction.SendFeedback);
                var feedbackObj = new JObject();
                feedbackObj.Add("fullName", txtName.Value);
                feedbackObj.Add("email", txtEmail.Value);
                feedbackObj.Add("type", cbSuggestion.IsChecked == true ? 1 : cbBug.IsChecked == true ? 2 : cbOther.IsChecked == true ? 3 : 0);
                feedbackObj.Add("subject", txtSubject.Value);
                feedbackObj.Add("message", txtMessage.Value);
                feedbackObj.Add("rating", txtRating.Value == null ? -1 : (int)txtRating.Value);
                var deviceObj = new JObject();
                deviceObj.Add("id", DeviceHelper.Id);
                deviceObj.Add("name", DeviceHelper.Name);
                deviceObj.Add("platform", DeviceHelper.Platform);
                deviceObj.Add("appVersion", DeviceHelper.AppVersion);
                feedbackObj.Add("device", deviceObj);
                requestObj.Add("feedback", feedbackObj);
                rootObj.Add("request", requestObj);

                string postData = rootObj.ToString();
                _byteArray = Encoding.UTF8.GetBytes(postData.ToCharArray());

                try
                {
                    var request = HttpWebRequest.Create(ApiHelper.ApiUrl);
                    request.Method = "POST";
                    request.ContentLength = _byteArray.Length;
                    request.ContentType = "application/json";
                    request.BeginGetRequestStream(SendDataCallBack, request);
                }
                catch (Exception)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        btnSend.IsActivated = true;
                        App.ShowProgress("geri bildiriminiz gönderilemedi, daha sonra tekrar deneyiniz", ProgressType.Error, ProgressTime.Normal);
                    });
                }
            }

        }

        private void SendDataCallBack(IAsyncResult result)
        {
            try
            {
                var request = result.AsyncState as HttpWebRequest;
                if (request != null)
                {
                    using (var stream = request.EndGetRequestStream(result))
                    {
                        stream.Write(_byteArray, 0, _byteArray.Length);
                        stream.Close();
                    }
                    request.BeginGetResponse(GetResponseCallBack, request);
                }
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    btnSend.IsActivated = true;
                    App.ShowProgress("geri bildiriminiz gönderilemedi, daha sonra tekrar deneyiniz", ProgressType.Error, ProgressTime.Normal);
                });
            }
        }

        private void GetResponseCallBack(IAsyncResult result)
        {
            try
            {
                var request = result.AsyncState as HttpWebRequest;
                if (request != null)
                {
                    var response = request.EndGetResponse(result);
                    var reader = new StreamReader(response.GetResponseStream());
                    var jsonResult = reader.ReadToEnd();
                    Populate(jsonResult);
                }
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    btnSend.IsActivated = true;
                    App.ShowProgress("geri bildiriminiz gönderilemedi, daha sonra tekrar deneyiniz", ProgressType.Error, ProgressTime.Normal);
                });
            }
        }

        private void Populate(string jsonResult)
        {
            var root = JObject.Parse(jsonResult);
            if (Convert.ToBoolean(root["status"].ToString()))
            {
                Dispatcher.BeginInvoke(() =>
                {
                    App.HideProgress();
                    App.ShowProgress("geri bildiriminiz alınmıştır, gerekli görülürse email adresinize dönüş yapılacaktır", ProgressType.Success, ProgressTime.Normal);
                    btnSend.IsActivated = true;
                    txtSubject.Value = null;
                    txtMessage.Value = null;
                    txtRating = null;
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    btnSend.IsActivated = true;
                    if (!string.IsNullOrEmpty(root["message"].ToString()))
                        App.ShowProgress(root["message"].ToString(), ProgressType.Error, ProgressTime.Normal);
                    else
                        App.ShowProgress("geri bildiriminiz gönderilemedi, daha sonra tekrar deneyiniz", ProgressType.Error, ProgressTime.Normal);
                });
            }
        }
    }
}