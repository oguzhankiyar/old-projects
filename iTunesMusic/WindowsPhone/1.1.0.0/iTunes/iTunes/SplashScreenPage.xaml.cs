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
using iTunes.Models;

namespace iTunes
{
    public partial class SplashScreenPage : PhoneApplicationPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SplashScreen();
            base.OnNavigatedTo(e);
        }
        async void SplashScreen()
        {
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            if (Database.Settings.InstallationCompleted)
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            else
                NavigationService.Navigate(new Uri("/InstallationPage.xaml", UriKind.Relative));
        }
    }
}