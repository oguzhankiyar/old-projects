using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Mobisis.Models;
using Mobisis.ObisisServiceReference;

namespace Mobisis
{
    public partial class PeriodsPage : PhoneApplicationPage
    {
        public PeriodsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageCanvas = new SwipeMenu(this, PageCanvas, LayoutRoot, MoveAnimation, LeftMenu);
            Container.Height = App.Current.RootVisual.RenderSize.Height;

            if (Database.Student == null)
            {
                MessageBox.Show("Giriş yapmanız gerekiyor..");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                Periods.ItemsSource = Database.Student.Programs[0].Periods;
                Periods.SelectedItem = null;
            }
            base.OnNavigatedTo(e);
        }

        private void Periods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Period selected = Periods.SelectedItem as Period;
            if (selected != null)
                NavigationService.Navigate(new Uri("/PeriodDetailsPage.xaml?YearCode=" + selected.YearCode + "&Code=" + selected.Code, UriKind.Relative));
        }
    }
}