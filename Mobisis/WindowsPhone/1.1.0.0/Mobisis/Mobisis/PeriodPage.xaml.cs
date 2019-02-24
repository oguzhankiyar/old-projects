using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using Mobisis.ServiceReference;

namespace Mobisis
{
    public partial class PeriodPage : PhoneApplicationPage
    {
        public PeriodPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Data.Connection == null)
            {
                MessageBox.Show("Giriş yapmanız gerekiyor!");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                PeriodList.ItemsSource = Data.Connection.Ogrenci.Donemler;
            }
        }
        private void PeriodList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Donem item = PeriodList.SelectedItem as Donem;
            NavigationService.Navigate(new Uri("/LessonPage.xaml?YearCode=" + item.OgretimYiliKodu + "&No=" + item.No, UriKind.Relative));
        }
    }
}