using System;
using System.Linq;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using TicketApp.Models;

namespace TicketApp.Pages
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

            if (!Database.SavedData.IsInstallationCompleted)
                NavigationService.Navigate(new Uri("/Pages/InstallationPage.xaml", UriKind.RelativeOrAbsolute));
            else
                App.SetTitle("Biletonly");
        }

        private void btnBus_Click(object sender, EventArgs e)
        {
            Logger.Clicked("btnBus");
            NavigationService.Navigate(new Uri("/Pages/Bus/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnAirplane_Click(object sender, EventArgs e)
        {
            Logger.Clicked("btnAirplane");
            NavigationService.Navigate(new Uri("/Pages/Airplane/SearchPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnTicketAction_Click(object sender, EventArgs e)
        {
            Logger.Clicked("btnTicketAction");
            NavigationService.Navigate(new Uri("/Pages/TicketAction/ListPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void imgInfo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Logger.Clicked("imgInfo");
            NavigationService.Navigate(new Uri("/Pages/Help/HomePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void imgIpekTr_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Logger.Clicked("imgIpekTr");
            var wbTask = new WebBrowserTask();
            wbTask.Uri = new Uri("http://www.ipektr.com", UriKind.RelativeOrAbsolute);
            wbTask.Show();
        }

        private void imgSettings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Logger.Clicked("imgSettings");
            NavigationService.Navigate(new Uri("/Pages/Setting/HomePage.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}