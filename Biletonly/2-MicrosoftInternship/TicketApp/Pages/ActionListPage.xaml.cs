using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Data;
using TicketApp.Entities;

namespace TicketApp.Pages
{
    public partial class ActionListPage : PhoneApplicationPage
    {
        public ActionListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ActionList.ItemsSource = Database.SavedData.Reservations.ToList();
            ActionList.SelectedItem = null;
        }

        private void ActionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ActionList.SelectedItem as Reservation;
            if (selected != null)
            {
                NavigationService.Navigate(new Uri("/Pages/ActionDetailsPage.xaml?PNR=" + selected.PNR, UriKind.RelativeOrAbsolute));
            }
        }


    }
}