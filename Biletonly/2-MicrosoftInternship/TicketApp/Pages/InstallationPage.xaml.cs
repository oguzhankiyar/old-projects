using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Biletall.Helper;
using Biletall.Helper.Entities;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicketApp.Controls.Enums;
using TicketApp.Models;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TicketApp.Pages
{
    public partial class InstallationPage : PhoneApplicationPage
    {
        bool _isCompleted = false;
        public InstallationPage()
        {
            InitializeComponent();
            App.SetTitle("Biletonly Kurulum");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.ClearBackHistory();

            btnSkip.Value = "Kuruluma Başla";
        }

        private async void btnSkip_Clicked(object sender, EventArgs e)
        {
            Logger.Clicked("btnSkip");
            if (_isCompleted)
            {
                Database.SavedData.IsInstallationCompleted = true;
                Database.Update();

                NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.RelativeOrAbsolute));
                App.ClearBackHistory();
            }
            else if (!App.IsInternetAvailable)
            {
                Logger.WriteLine("internet is unavailable");
                tbInfo.Text = "";
                AppendInfo("internet bağlantınızı kontrol ediniz");
            }
            else
            {
                Logger.WriteLine("setup is starting");
                btnSkip.IsActivated = false;
                tbInfo.Text = "";
                App.ShowProgress();
                AppendInfo("kurulum başlatılıyor..");
                RegisterDevice();
            }
        }

        private void RegisterDevice()
        {
            Logger.MethodCalled("RegisterDevice()");
            AppendInfo("cihaz uyumluluğu denetleniyor..");
            AppendInfo("cihaz kaydı açılıyor..");
            var request = HttpWebRequest.Create(ApiHelper.ApiUrl);
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.BeginGetRequestStream(SendDataCallBack, request);
        }

        private void SendDataCallBack(IAsyncResult result)
        {
            Logger.WriteLine("sending data...");
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                var obj = new JObject();
                obj.Add("version", Constraints.ApiVersion);
                obj.Add("username", Constraints.ApiUsername);
                obj.Add("password", Constraints.ApiPassword);

                var requestObj = new JObject();
                requestObj.Add("type", ApiAction.DeviceRegister);
                var deviceObj = new JObject();
                deviceObj.Add("id", DeviceHelper.Id);
                deviceObj.Add("name", DeviceHelper.Name);
                deviceObj.Add("platform", DeviceHelper.Platform);
                deviceObj.Add("appVersion", DeviceHelper.AppVersion);
                requestObj.Add("device", deviceObj);
                obj.Add("request", requestObj);
                byte[] byteArray = Encoding.UTF8.GetBytes(obj.ToString());
                using (var stream = request.EndGetRequestStream(result))
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Close();
                }
                request.BeginGetResponse(GetResponseCallBack, request);
            }
        }

        private async void GetResponseCallBack(IAsyncResult result)
        {
            Logger.WriteLine("getting response...");
            var request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    var response = request.EndGetResponse(result);
                    var reader = new StreamReader(response.GetResponseStream());
                    string json = reader.ReadToEnd();
                    var obj = JObject.Parse(json);
                    if (Convert.ToBoolean(obj["status"].ToString()))
                    {
                        Logger.WriteLine("device is registered");
                        AppendInfo("cihaz kaydedildi.");
                        await LoadStations();
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            AppendInfo("kurulum tamamlanamadı, tekrar deneyiniz");
                            btnSkip.IsActivated = true;
                            btnSkip.Value = "Yeniden Dene";
                            App.HideProgress();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        AppendInfo("kurulum tamamlanamadı, tekrar deneyiniz");
                        btnSkip.IsActivated = true;
                        btnSkip.Value = "Yeniden Dene";
                        App.HideProgress();
                    });
                }
            }
        }

        private async Task LoadStations()
        {
            Logger.MethodCalled("LoadStations()");
            Database.SavedData.StationsUpdatedDate = DateTime.Now;
            string fileContent;
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            Stream fileStream = await folder.OpenStreamForReadAsync("ticketapp.data.txt");
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                fileContent = streamReader.ReadToEnd();
            }

            Logger.WriteLine("loaded saved station data");

            string json = fileContent;
            var jobj = JObject.Parse(json);
            AppendInfo("istasyonlar yükleniyor..");
            var stations = JsonConvert.DeserializeObject<List<Station>>(jobj["Stations"].ToString());
            Database.SavedData.Stations = stations;
            AppendInfo("istasyonlar yüklendi.");
            AppendInfo("havalimanları yükleniyor..");
            var airports = JsonConvert.DeserializeObject<List<Station>>(jobj["Airports"].ToString());
            Database.SavedData.Airports = airports;
            AppendInfo("havalimanları yüklendi.");
            Database.Update();
            AppendInfo("veriler kaydedildi.");
            AppendInfo("kurulum tamamlandı.");
            Dispatcher.BeginInvoke(() =>
            {
                Logger.WriteLine("setup is completed");
                _isCompleted = true;
                App.HideProgress();
                btnSkip.Value = "Uygulamaya Geç";
                btnSkip.IsActivated = true;
            });
        }

        private void AppendInfo(string text)
        {
            Logger.MethodCalled("AppendInfo(" + text + ")");
            Dispatcher.BeginInvoke(() =>
            {
                tbInfo.Text += text + Environment.NewLine;
            });
        }
    }
}