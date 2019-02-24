using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Mobisis.ServiceReference;
using System.Net.NetworkInformation;
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

            DataSaver<Baglanti> dataSaver = new DataSaver<Baglanti>();
            Data.Connection = dataSaver.LoadMyData("Connection");

            DataSaver<string> _dataSaver = new DataSaver<string>();
            string id = _dataSaver.LoadMyData("StudentID");
            string pass = _dataSaver.LoadMyData("Password");
            if (id != null && pass != null)
            {
                txtStudentID.Text = _dataSaver.LoadMyData("StudentID");
                txtPassword.Password = _dataSaver.LoadMyData("Password");
            }
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
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
                    if (cbRememberMe.IsChecked == true)
                    {
                        DataSaver<string> _dataSaver = new DataSaver<string>();
                        _dataSaver.SaveMyData(txtStudentID.Text, "StudentID");
                        _dataSaver.SaveMyData(txtPassword.Password, "Password");
                    }
                    btnLogin.IsEnabled = false;
                    ProgressIndicator indicator = new ProgressIndicator();
                    indicator.IsIndeterminate = true;
                    indicator.IsVisible = true;
                    indicator.Text = "Giriş yapılıyor..";
                    SystemTray.SetProgressIndicator(this, indicator);
                    Service1Client client = new Service1Client();
                    client.LoginAsync(txtStudentID.Text, txtPassword.Password, "WP8");
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