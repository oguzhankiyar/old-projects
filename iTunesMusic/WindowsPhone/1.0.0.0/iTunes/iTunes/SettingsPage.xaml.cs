using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iTunes.Models;
using iTunes.Resources;
using System.Reflection;

namespace iTunes
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private int SelectionCount = 0;
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.MatchOverriddenTheme();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBarMenuItem MenuItem_Settings = new ApplicationBarMenuItem();
            MenuItem_Settings.Text = AppResources.HomePageLabel;
            MenuItem_Settings.Click += new EventHandler(GoHomePage);
            ApplicationBar.MenuItems.Add(MenuItem_Settings);

            Settings.DataContext = Database.Settings;
            Countries.ItemsSource = Database.Countries;
            Countries.SelectedItem = Database.Settings.DefaultCountry;
            AppVersion.Text = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version.ToString();
            base.OnNavigatedTo(e);
        }

        private void GoHomePage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Countries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (++SelectionCount > 1)
                Database.Settings.DefaultCountry = Countries.SelectedItem as Country;
        }
    }
}