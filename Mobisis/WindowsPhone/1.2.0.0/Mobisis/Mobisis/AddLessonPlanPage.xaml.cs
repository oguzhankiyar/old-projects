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
    public partial class AddLessonPlanPage : PhoneApplicationPage
    {
        public int index = 0;
        public AddLessonPlanPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageCanvas = new SwipeMenu(this, PageCanvas, LayoutRoot, MoveAnimation, LeftMenu);
            Container.Height = App.Current.RootVisual.RenderSize.Height;

            if (NavigationContext.QueryString.ContainsKey("index"))
                index = Convert.ToInt32(NavigationContext.QueryString["index"].ToString());

            List<string> days = new List<string>();
            days.Add("Pazartesi");
            days.Add("Salı");
            days.Add("Çarşamba");
            days.Add("Perşembe");
            days.Add("Cuma");
            Lessons.ItemsSource = Database.Student.Programs[0].Periods.First().Lessons;
            Days.ItemsSource = days;
            Days.SelectedIndex = index;
            base.OnNavigatedTo(e);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            LessonPlanItem lesson = new LessonPlanItem();
            lesson.Lesson = Lessons.SelectedItem as Lesson;
            switch (Days.SelectedIndex)
            {
                case 0:
                    lesson.Day = DayOfWeek.Monday;
                    break;
                case 1:
                    lesson.Day = DayOfWeek.Tuesday;
                    break;
                case 2:
                    lesson.Day = DayOfWeek.Wednesday;
                    break;
                case 3:
                    lesson.Day = DayOfWeek.Thursday;
                    break;
                case 4:
                    lesson.Day = DayOfWeek.Friday;
                    break;
            }
            lesson.BeginDateTime = (DateTime)BeginDateTime.Value;
            lesson.EndDateTime = (DateTime)EndDateTime.Value;
            Database.LessonPlan.Add(lesson);
            NavigationService.Navigate(new Uri("/LessonPlanPage.xaml?index=" + Days.SelectedIndex, UriKind.Relative));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LessonPlanPage.xaml?index=" + index, UriKind.Relative));
        }

        private void BeginDateTime_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (Lessons != null)
            {
                Lesson lesson = Lessons.SelectedItem as Lesson;
                DateTime beginDate = (DateTime)BeginDateTime.Value;
                EndDateTime.Value = beginDate.AddHours(Math.Ceiling(lesson.Credit));
            }
        }
    }
}