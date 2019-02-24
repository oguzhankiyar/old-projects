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
    public partial class CalculateGanoPage : PhoneApplicationPage
    {
        public CalculateGanoPage()
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
                List<Lesson> lessons = new List<Lesson>();
                Program program = Database.Student.Programs[0];
                GANO.Text = program.GANO.ToString();
                lessons = program.Periods.First().Lessons.ToList();
                CalculateGANO.Lessons = new List<Lesson>();
                foreach (var lesson in lessons)
                {
                    Lessons.Children.Add(CalculateGANO.GetLessonPanel(lesson));
                }
            }
            base.OnNavigatedTo(e);
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            GANO.Text = CalculateGANO.Calculate().ToString();
        }
    }
}