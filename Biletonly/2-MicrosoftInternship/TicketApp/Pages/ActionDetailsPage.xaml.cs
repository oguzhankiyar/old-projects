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
using TicketApp.Requests;

namespace TicketApp.Pages
{
    public partial class ActionDetailsPage : PhoneApplicationPage
    {
        Reservation reservation;

        public ActionDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string pnr = NavigationContext.QueryString["PNR"] as string;
            reservation = Database.SavedData.Reservations.SingleOrDefault(x => x.PNR == pnr);
            LayoutRoot.DataContext = reservation;
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            ReservationRequest.Cancel(reservation);
            ReservationRequest.OnCanceled += (r) =>
                {
                    if (r)
                        MessageBox.Show("Rezervasyon iptal işlemi başarıyla gerçekleşti.", "Başarılı!", MessageBoxButton.OK);
                    else
                        MessageBox.Show("Rezervasyon iptal işlemi gerçekleşemedi.", "Başarısız!", MessageBoxButton.OK);
                    NavigationService.Navigate(new Uri("/Pages/ActionListPage.xaml", UriKind.RelativeOrAbsolute));
                };
        }

        private void Buy_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Bus/CreditCardPage.xaml?PNR=" + reservation.PNR, UriKind.RelativeOrAbsolute));
        }
    }
}