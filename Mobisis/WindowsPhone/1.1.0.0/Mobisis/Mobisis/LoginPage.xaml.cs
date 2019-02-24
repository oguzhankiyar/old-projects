using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Mobisis.ServiceReference;
using System.IO.IsolatedStorage;

namespace Mobisis
{
    public partial class LoginPage : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Data.StudentID == null && Data.Password == null)
            {
                DataSaver<Baglanti> dataSaver = new DataSaver<Baglanti>();
                Data.Connection = dataSaver.LoadMyData("Connection");

                DataSaver<string> _dataSaver = new DataSaver<string>();
                Data.StudentID = _dataSaver.LoadMyData("StudentID");
                Data.Password = _dataSaver.LoadMyData("Password");
            }
            if (Data.StudentID != null && Data.Password != null)
            {
                txtStudentID.Text = Data.StudentID;
                txtPassword.Password = Data.Password;
                cbRememberMe.IsChecked = Data.IsRemember;
            }
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                {
                    MessageBox.Show("İnternet bağlantınızı kontrol ediniz!");
                    NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                }
                else if (string.IsNullOrEmpty(txtStudentID.Text) || string.IsNullOrEmpty(txtPassword.Password))
                {
                    MessageBox.Show("Boş alan bıraktınız! Tekrar deneyiniz..");
                    NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                }
                else
                {
                    txtStudentID.IsEnabled = false;
                    txtPassword.IsEnabled = false;
                    btnLogin.IsEnabled = false;
                    cbRememberMe.IsEnabled = false;
                    ProgressIndicator indicator = new ProgressIndicator();
                    indicator.IsIndeterminate = true;
                    indicator.IsVisible = true;
                    indicator.Text = "Giriş yapılıyor..";
                    SystemTray.SetProgressIndicator(this, indicator);

                    Data.StudentID = txtStudentID.Text;
                    Data.Password = txtPassword.Password;
                    Data.IsRemember = (bool)cbRememberMe.IsChecked;
                    if (cbRememberMe.IsChecked == true)
                    {
                        DataSaver<string> _dataSaver = new DataSaver<string>();
                        _dataSaver.SaveMyData(Data.StudentID, "StudentID");
                        _dataSaver.SaveMyData(Data.Password, "Password");
                    }
                    else
                    {
                        DataSaver<string> _dataSaver = new DataSaver<string>();
                        _dataSaver.SaveMyData(null, "StudentID");
                        _dataSaver.SaveMyData(null, "Password");
                    }

                    Service1Client client = new Service1Client();
                    client.LoginAsync(txtStudentID.Text, txtPassword.Password, Data.Version);
                    client.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(Completed);
                }
            }
            catch (Exception)
            {
                Data.Connection = null;
                MessageBox.Show("Sunucuya bağlanılamıyor..");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }

        }
        private void Completed(object sender, LoginCompletedEventArgs e)
        {
            SystemTray.ProgressIndicator.IsVisible = false;
            Data.Connection = e.Result;
            if (Data.Connection == null)
            {
                MessageBox.Show("Sunucuya bağlanılamıyor..");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                if (Data.Connection.Mesaj != null)
                {
                    MessageBox.Show(Data.Connection.Mesaj);
                }
                if (Data.Connection.Durum == false)
                {
                    if (Data.Connection.Mesaj == null)
                    {
                        MessageBox.Show("Bilgileriniz hatalı! Tekrar deneyiniz..");
                    }
                    Data.Connection = null;
                }
                else
                {
                    if (cbRememberMe.IsChecked == true)
                    {
                        DataSaver<Baglanti> dataSaver = new DataSaver<Baglanti>();
                        dataSaver.SaveMyData(Data.Connection, "Connection");
                    }
                    NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
                }
            }
        }
    }
}