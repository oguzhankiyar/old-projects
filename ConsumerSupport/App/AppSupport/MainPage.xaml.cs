using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AppSupport.Resources;
using AppSupport.SupportService;

namespace AppSupport
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ConsumerSupportServiceClient client = new ConsumerSupportServiceClient();
            Admin admin = new Admin() { Username = "ogzkyr", Password = "API_PASSWORD" };
            client.ListSupportsAsync(admin);
            client.ListSupportsCompleted += client_ListSupportsCompleted;
            base.OnNavigatedTo(e);
        }

        void client_ListSupportsCompleted(object sender, ListSupportsCompletedEventArgs e)
        {
            SupportList supports = e.Result as SupportList;
            SupportList.ItemsSource = supports.List.ToList();
        }
    }
}