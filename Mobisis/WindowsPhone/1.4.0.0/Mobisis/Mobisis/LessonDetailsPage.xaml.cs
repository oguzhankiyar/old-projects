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
    public partial class LessonDetailsPage : PhoneApplicationPage
    {
        public LessonDetailsPage()
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
                int periodCode = NavigationContext.QueryString.ContainsKey("PeriodCode") ? Convert.ToInt32(NavigationContext.QueryString["PeriodCode"]) : 0;
                int periodYearCode = NavigationContext.QueryString.ContainsKey("PeriodYearCode") ? Convert.ToInt32(NavigationContext.QueryString["PeriodYearCode"]) : 0;
                string code = NavigationContext.QueryString.ContainsKey("Code") ? NavigationContext.QueryString["Code"].ToString() : null;
                Period period = Database.Student.Programs[0].Periods.SingleOrDefault(x => x.Code == periodCode && x.YearCode == periodYearCode);
                Container.DataContext = period == null ? null : period.Lessons.SingleOrDefault(x => x.Code == code);
            }
            base.OnNavigatedTo(e);
        }
    }
}