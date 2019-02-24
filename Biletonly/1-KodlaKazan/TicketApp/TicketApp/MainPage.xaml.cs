using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TicketApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var backStack = NavigationService.BackStack.FirstOrDefault();
            if (backStack != null)
                if (backStack.Source.OriginalString == "/SplashScreenPage.xaml")
                    NavigationService.RemoveBackEntry();
        }

        private void Search_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
        }

        private void Actions_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MyActionsPage.xaml", UriKind.Relative));
        }
    }
}