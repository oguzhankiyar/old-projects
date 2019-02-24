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
    public partial class LessonPlanPage : PhoneApplicationPage
    {
        public LessonPlanPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageCanvas = new SwipeMenu(this, PageCanvas, LayoutRoot, MoveAnimation, LeftMenu);
            Container.Height = App.Current.RootVisual.RenderSize.Height;

            ApplicationBar.MatchOverriddenTheme();
            if (Database.Student == null)
            {
                MessageBox.Show("Giriş yapmanız gerekiyor..");
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                MondayPlan.ItemsSource = Database.LessonPlan.Where(x => x.Day == DayOfWeek.Monday).OrderBy(x => x.BeginDateTime.Hour).ToList();
                TuesdayPlan.ItemsSource = Database.LessonPlan.Where(x => x.Day == DayOfWeek.Tuesday).OrderBy(x => x.BeginDateTime.Hour).ToList();
                WednesdayPlan.ItemsSource = Database.LessonPlan.Where(x => x.Day == DayOfWeek.Wednesday).OrderBy(x => x.BeginDateTime.Hour).ToList();
                ThursdayPlan.ItemsSource = Database.LessonPlan.Where(x => x.Day == DayOfWeek.Thursday).OrderBy(x => x.BeginDateTime.Hour).ToList();
                FridayPlan.ItemsSource = Database.LessonPlan.Where(x => x.Day == DayOfWeek.Friday).OrderBy(x => x.BeginDateTime.Hour).ToList();
                if (MondayPlan.ItemsSource.Count == 0)
                {
                    MondayPlan.Visibility = Visibility.Collapsed;
                    MondayEmptyMessage.Visibility = Visibility.Visible;
                }
                if (TuesdayPlan.ItemsSource.Count == 0)
                {
                    TuesdayPlan.Visibility = Visibility.Collapsed;
                    TuesdayEmptyMessage.Visibility = Visibility.Visible;
                }
                if (WednesdayPlan.ItemsSource.Count == 0)
                {
                    WednesdayPlan.Visibility = Visibility.Collapsed;
                    WednesdayEmptyMessage.Visibility = Visibility.Visible;
                }
                if (ThursdayPlan.ItemsSource.Count == 0)
                {
                    ThursdayPlan.Visibility = Visibility.Collapsed;
                    ThursdayEmptyMessage.Visibility = Visibility.Visible;
                }
                if (FridayPlan.ItemsSource.Count == 0)
                {
                    FridayPlan.Visibility = Visibility.Collapsed;
                    FridayEmptyMessage.Visibility = Visibility.Visible;
                }
                if (NavigationContext.QueryString.ContainsKey("index"))
                {
                    LessonPlan.SelectedIndex = Convert.ToInt32(NavigationContext.QueryString["index"].ToString());
                }
            }
            base.OnNavigatedTo(e);
        }

        private void AddLesson_Click(object sender, EventArgs e)
        {
            int index = LessonPlan.SelectedIndex;
            NavigationService.Navigate(new Uri("/AddLessonPlanPage.xaml?index=" + index, UriKind.Relative));
        }

        private void DeleteLesson_Click(object sender, RoutedEventArgs e)
        {
            LessonPlanItem selectedItem = (sender as MenuItem).DataContext as LessonPlanItem;
            Database.LessonPlan.Remove(selectedItem);
            Database.UpdateLessonPlan();
            int index = LessonPlan.SelectedIndex;
            NavigationService.Navigate(new Uri("/LessonPlanPage.xaml?date=" + DateTime.Now + "&index=" + index, UriKind.Relative));
        }
    }
}