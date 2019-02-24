using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CargoTracking.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace CargoTracking
{
    public partial class HistoryPage : PhoneApplicationPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            History.ItemsSource = Database.History.OrderByDescending(x => x.LastSearchDate).ToList();
            if (Database.History.Any())
            {
                WarningText.Visibility = Visibility.Collapsed;
                History.Visibility = Visibility.Visible;
            }
            base.OnNavigatedTo(e);
        }

        private void Back_Tap(object sender, GestureEventArgs e)
        {
            if (!string.IsNullOrEmpty(Database.TrackingCode) && Database.SelectedFactory != null)
                NavigationService.Navigate(new Uri("/TrackingPage.xaml?trackingCode=" + Database.TrackingCode + "&factoryName=" + Database.SelectedFactory.Name, UriKind.Relative));
            else
                NavigationService.Navigate(new Uri("/TrackingPage.xaml", UriKind.Relative));
        }

        private void History_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = History.SelectedItem as Tracking;
            if (selected != null)
                NavigationService.Navigate(new Uri("/TrackingPage.xaml?trackingCode=" + selected.Code + "&factoryName=" + selected.Factory.Name, UriKind.Relative));
        }

        private void RemoveFromHistory_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext as Tracking;
            Database.RemoveFromHistory(selected);
            History.ItemsSource = Database.History;
            if (!Database.History.Any())
            {
                History.Visibility = Visibility.Collapsed;
                WarningText.Visibility = Visibility.Visible;
            }
        }
    }
}