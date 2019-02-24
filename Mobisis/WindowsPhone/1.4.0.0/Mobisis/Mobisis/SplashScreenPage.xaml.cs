using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Mobisis.Models;

namespace Mobisis
{
    public partial class SplashScreenPage : PhoneApplicationPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Function.StartTask();
            SplashScreen();
            base.OnNavigatedTo(e);
        }
        async void SplashScreen()
        {
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            if (Database.Student != null)
                NavigationService.Navigate(new Uri("/StudentPage.xaml", UriKind.Relative));
            else
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }
    }
}