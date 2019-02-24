using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace TicketApp.Pages.Help
{
    public partial class ReservationPage : PhoneApplicationPage
    {
        public ReservationPage()
        {
            InitializeComponent();
            App.SetTitle("Yardım");
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.biletall.com.tr", UriKind.RelativeOrAbsolute);
            webBrowserTask.Show();
        }
    }
}