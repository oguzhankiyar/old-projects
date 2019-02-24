using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.Mobisis.Data;
using OK.Mobisis.Entities.ModelEntities;
using OK.Mobisis.Api.Helper.Parsings;
using OK.Mobisis.Api.Helper.Requests;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OK.Mobisis.Api.Helper;
using OK.Mobisis.Api.Helper.Enums;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OK.Mobisis.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            new SwipeMenu().AddTo(this);

            Prepare();
        }

        private void Prepare()
        {
            if (Database.TempData.Student != null)
            {
                txtStudentId.Text = Database.TempData.Student.Id;
                txtPassword.Password = Database.TempData.Student.Password;
                if (Database.TempData.Student == Database.SavedData.Student)
                    cbRememberMe.IsChecked = true;
            }
            else if (Database.SavedData.Student != null)
            {
                txtStudentId.Text = Database.SavedData.Student.Id;
                txtPassword.Password = Database.SavedData.Student.Password;
                cbRememberMe.IsChecked = true;
            }

            txtStudentId.GotFocus += txtStudentId_GotFocus;
            txtStudentId.LostFocus += txtStudentId_LostFocus;
            txtPassword.GotFocus += txtPassword_GotFocus;
            txtPassword.LostFocus += txtPassword_LostFocus;
            txtStudentIdPanel.Tapped += txtStudentIdPanel_Tapped;
            txtPasswordPanel.Tapped += txtPasswordPanel_Tapped;
        }

        void txtStudentIdPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtStudentId.Focus(FocusState.Keyboard);
        }

        private void txtPasswordPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtPassword.Focus(FocusState.Keyboard);
        }

        void txtStudentId_LostFocus(object sender, RoutedEventArgs e)
        {
            txtStudentId.Foreground = new SolidColorBrush(Colors.White);
        }
        
        void txtStudentId_GotFocus(object sender, RoutedEventArgs e)
        {
            txtStudentId.Foreground = new SolidColorBrush(Colors.Gray);
        }

        void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.Foreground = new SolidColorBrush(Colors.White);
        }

        void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                await new MessageDialog("İnternet bağlantınızı kontrol ediniz").ShowAsync();
            }
            else
            {
                string studentId = txtStudentId.Text;
                string password = txtPassword.Password;
                if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(password))
                {
                    await new MessageDialog("Boş bıraktığınız alan(lar) var").ShowAsync();
                }
                else
                {
                    LayoutRoot.ShowStatus("giriş yapılıyor..");
                    StudentRequests.StudentRequest.OnCompleted = GetStudent_Completed;
                    StudentRequests.GetStudent(studentId, password);
                }
            }
        }

        private async void GetStudent_Completed(BaseResponse<Student> studentResponse)
        {
            LayoutRoot.HideStatus();
            if (studentResponse.Status == ResponseStatus.Successful)
            {
                Database.TempData.Reset();
                var student = studentResponse.Result as Student;
                Database.TempData.Student = student;

                if (cbRememberMe.IsChecked == true)
                {
                    Database.TempData.RememberMe = true;
                    Database.SavedData.Student = student;
                    await Database.Update();
                }

                if (!string.IsNullOrEmpty(studentResponse.Message))
                    await new MessageDialog(studentResponse.Message).ShowAsync();

                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.Frame.Navigate(typeof(StudentPage));
                });
            }
            else if (!string.IsNullOrEmpty(studentResponse.Message))
                    await new MessageDialog(studentResponse.Message).ShowAsync();
            else
                await new MessageDialog("Giriş Başarısız!").ShowAsync();
        }
        
    }
}
