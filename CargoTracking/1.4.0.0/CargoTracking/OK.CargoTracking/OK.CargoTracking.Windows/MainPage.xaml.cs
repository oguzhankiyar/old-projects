using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.CargoTracking.Helper;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OK.CargoTracking.Windows.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OK.CargoTracking.Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            prepare();
        }

        private void prepare()
        {
            txtInfo.Text = "yükleniyor...";
            if (!Database.SavedData.IsDeviceRegistered)
                deviceRegister();
            else if (!Database.SavedData.Factories.Any())
                updateFactories();
            else
                checkMessages();
        }

        private async void deviceRegister()
        {
            try
            {
                txtInfo.Text = "cihaz kaydediliyor...";
                if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                {
                    txtInfo.Text = "İnternet bağlantısını kontrol ediniz.\nTekrar denemek için dokunun.";
                    txtInfo.Tap += TxtInfo_Tap;
                    return;
                }
                Global.Token = CryptoHelper.CreateToken(DeviceHelper.Id);
                var registerResponse = await DeviceRequests.RegisterDeviceAsync();
                if (registerResponse.Status)
                {
                    Database.SavedData.IsDeviceRegistered = true;
                    Database.Update();
                    prepare();
                }
                else
                {
                    txtInfo.Text = "Bir sorun oluştu.\nTekrar denemek için dokunun.";
                    txtInfo.Tap += TxtInfo_Tap;
                }
            }
            catch (Exception)
            {
                txtInfo.Text = "Bir sorun oluştu.\nTekrar denemek için dokunun.";
                txtInfo.Tap += TxtInfo_Tap;
            }
        }

        private void TxtInfo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            txtInfo.Tap -= TxtInfo_Tap;
            prepare();
        }

        private async void updateFactories()
        {
            try
            {
                txtInfo.Text = "şirketler yükleniyor...";
                if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                {
                    txtInfo.Text = "İnternet bağlantısını kontrol ediniz.\nTekrar denemek için dokunun.";
                    txtInfo.Tap += TxtInfo_Tap;
                    return;
                }
                Global.Token = CryptoHelper.CreateToken(DeviceHelper.Id);
                var factoriesResponse = await FactoryRequests.GetFactoriesAsync();
                if (factoriesResponse.Status)
                {
                    var factories = factoriesResponse.Result as List<Factory>;
                    if (factories != null)
                        Database.UpdateFactories(factories);
                    prepare();
                }
                else
                {
                    txtInfo.Text = "Bir sorun oluştu.\nTekrar denemek için dokunun.";
                    txtInfo.Tap += TxtInfo_Tap;
                }
            }
            catch (Exception)
            {
                txtInfo.Text = "Bir sorun oluştu.\nTekrar denemek için dokunun.";
                txtInfo.Tap += TxtInfo_Tap;
            }
        }

        private async void checkMessages()
        {
            try
            {
                txtInfo.Text = "mesajlar güncelleniyor...";
                Global.Token = CryptoHelper.CreateToken(DeviceHelper.Id);
                var messagesResponse = await MessageRequests.GetMessagesAsync();
                if (messagesResponse.Status)
                {
                    var messages = messagesResponse.Result as List<Message>;
                    if (messages != null && messages.Any())
                    {
                        foreach (var message in messages)
                        {
                            Database.AddMessage(message);
                        }
                    }
                }
            }
            catch (Exception) { }
            complete();
        }

        private void complete()
        {
            this.NavigationService.Navigate(new Uri("/TrackingPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
