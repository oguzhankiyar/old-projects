using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TicketApp.Pages.Help
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
            App.SetTitle("Yardım");
        }
        
        private void Item_Tap(object sender, GestureEventArgs e)
        {
            var panel = sender as StackPanel;
            string tag = panel.Tag as string;
            switch (tag)
            {
                case "About":
                    NavigationService.Navigate(new Uri("/Pages/Help/AboutPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Search":
                    NavigationService.Navigate(new Uri("/Pages/Help/SearchPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "List":
                    NavigationService.Navigate(new Uri("/Pages/Help/ListPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Select":
                    NavigationService.Navigate(new Uri("/Pages/Help/SelectPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Details":
                    NavigationService.Navigate(new Uri("/Pages/Help/DetailsPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Passengers":
                    NavigationService.Navigate(new Uri("/Pages/Help/PassengersPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Reservation":
                    NavigationService.Navigate(new Uri("/Pages/Help/ReservationPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Buying":
                    NavigationService.Navigate(new Uri("/Pages/Help/BuyingPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "AfterBuying":
                    NavigationService.Navigate(new Uri("/Pages/Help/AfterBuyingPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Laws":
                    NavigationService.Navigate(new Uri("/Pages/Help/LawsPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Feedback":
                    NavigationService.Navigate(new Uri("/Pages/Help/FeedbackPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case "Contact":
                    NavigationService.Navigate(new Uri("/Pages/Help/ContactPage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                default:
                    break;
            }
        }
    }
}