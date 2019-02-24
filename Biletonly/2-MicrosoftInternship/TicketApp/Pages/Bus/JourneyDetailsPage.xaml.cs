using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TicketApp.Controls.Enums;
using TicketApp.Models;

namespace TicketApp.Pages.Bus
{
    public partial class JourneyDetailsPage : PhoneApplicationPage
    {
        public JourneyDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.SetTitle("Sefer Detayları");

            LayoutRoot.DataContext = Database.TempData.Ticket.Journeys[0];

            DateTime departureDate = Database.TempData.Ticket.Journeys[0].Segments[0].DepartureDate;
            if (departureDate.Hour <= 6)
            {
                string text = "otobüs " + departureDate.GetDescription() + " bağlayan gece kalkacaktır";
                App.ShowProgress(text, ProgressType.Warning, ProgressTime.Normal);
            }
        }

        private void btnSelectSeat_Click(object sender, EventArgs e)
        {
            Logger.Clicked("btnSelectSeat");
            btnSelectSeat.IsActivated = false;
            App.ShowProgress("koltuklar listeleniyor...");
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (c, r) =>
            {
                btnSelectSeat.IsActivated = true;
                App.HideProgress();
                timer.Stop();
                NavigationService.Navigate(new Uri("/Pages/Bus/SeatListPage.xaml", UriKind.RelativeOrAbsolute));
            };
            timer.Start();
        }
    }
}