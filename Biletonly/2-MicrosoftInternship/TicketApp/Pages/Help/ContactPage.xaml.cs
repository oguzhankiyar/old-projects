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
    public partial class ContactPage : PhoneApplicationPage
    {
        public ContactPage()
        {
            InitializeComponent();
            App.SetTitle("Yardım");
        }

        private void Mail_Click(object sender, RoutedEventArgs e)
        {
            var emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = "destek@biletonly.com";
            emailComposeTask.Show();
        }
        private void Site_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.ogzkyr.net", UriKind.RelativeOrAbsolute);
            webBrowserTask.Show();
        }
        private void Facebook_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.facebook.com/oguzhan.kiyar", UriKind.RelativeOrAbsolute);
            webBrowserTask.Show();
        }
        private void Twitter_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.twitter.com/ogzkyr", UriKind.RelativeOrAbsolute);
            webBrowserTask.Show();
        }
        private void Linkedin_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://tr.linkedin.com/in/ogzkyr", UriKind.RelativeOrAbsolute);
            webBrowserTask.Show();
        }
    }
}