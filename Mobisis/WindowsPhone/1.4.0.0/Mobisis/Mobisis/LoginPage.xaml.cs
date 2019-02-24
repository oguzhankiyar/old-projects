using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Mobisis.Resources;
using Mobisis.ObisisServiceReference;
using Mobisis.Models;

namespace Mobisis
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var backStack = NavigationService.BackStack.FirstOrDefault();
            if (backStack != null)
                if (backStack.Source.OriginalString == "/SplashScreenPage.xaml")
                    NavigationService.RemoveBackEntry();

            PageCanvas = new SwipeMenu(this, PageCanvas, LayoutRoot, MoveAnimation, LeftMenu);
            Container.Height = App.Current.RootVisual.RenderSize.Height;

            if (Database.Student != null)
            {
                txtStudentID.Text = Database.Student.ID.ToString();
                txtPassword.Password = Database.Student.Password;
                var passwordEmpty = string.IsNullOrEmpty(txtPassword.Password);
                txtPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
                txtPassword.Opacity = passwordEmpty ? 0 : 100;
            }

            base.OnNavigatedTo(e);
        }
        private void txtPasswordLostFocus(object sender, RoutedEventArgs e)
        {
            var passwordEmpty = string.IsNullOrEmpty(txtPassword.Password);
            txtPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
            txtPassword.Opacity = passwordEmpty ? 0 : 100;
        }
        private void txtPasswordGotFocus(object sender, RoutedEventArgs e)
        {
            txtPasswordWatermark.Opacity = 0;
            txtPassword.Opacity = 100;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
            {
                MessageBox.Show("İnternet bağlantınızı kontrol ediniz!");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else if (string.IsNullOrEmpty(txtStudentID.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Boş bıraktığınız alan(lar) var!");
            }
            else
            {
                try
                {
                    ProgressIndicator indicator = new ProgressIndicator();
                    indicator.IsIndeterminate = true;
                    indicator.IsVisible = true;
                    indicator.Text = "Giriş yapılıyor..";

                    int studentID = Convert.ToInt32(txtStudentID.Text);
                    string password = txtPassword.Password;

                    ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                    client.GetStudentAsync(studentID, password);
                    client.GetStudentCompleted += client_GetStudentCompleted;
                    SystemTray.SetProgressIndicator(this, indicator);
                }
                catch (Exception)
                {
                    if (SystemTray.ProgressIndicator != null)
                        SystemTray.ProgressIndicator.IsVisible = false; 
                    MessageBox.Show("Sunucuya bağlanırken bir sorun oluştu!\nDaha sonra tekrar deneyiniz..");
                }
            }
        }
        private void client_GetStudentCompleted(object sender, GetStudentCompletedEventArgs e)
        {
            if (SystemTray.ProgressIndicator != null)
                SystemTray.ProgressIndicator.IsVisible = false; 
            Connection con = e.Result;
            if (con.Message != null)
                MessageBox.Show(con.Message);
            if (con.State)
            {
                Student student = con.DataObject as Student;
                Database.Student = student;
                if (cbRememberMe.IsChecked == true)
                    Database.UpdateStudent();
                NavigationService.Navigate(new Uri("/StudentPage.xaml", UriKind.Relative));
            }
        }
    }
}