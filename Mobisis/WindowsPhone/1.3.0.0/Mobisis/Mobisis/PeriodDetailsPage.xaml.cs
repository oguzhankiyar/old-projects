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
    public partial class PeriodDetailsPage : PhoneApplicationPage
    {
        private int code;
        private int yearCode;
        public PeriodDetailsPage()
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
                code = NavigationContext.QueryString.ContainsKey("Code") ? Convert.ToInt32(NavigationContext.QueryString["Code"]) : 0;
                yearCode = NavigationContext.QueryString.ContainsKey("YearCode") ? Convert.ToInt32(NavigationContext.QueryString["YearCode"]) : 0;
                Period currentPeriod = Database.Student.Programs[0].Periods.First();
                if ((code == 0 && yearCode == 0) || (code == currentPeriod.Code && yearCode == currentPeriod.YearCode))
                {
                    code = currentPeriod.Code;
                    yearCode = currentPeriod.YearCode;
                    Container.DataContext = currentPeriod;
                    if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None)
                    {
                        try
                        {
                            ProgressIndicator indicator = new ProgressIndicator();
                            indicator.IsIndeterminate = true;
                            indicator.IsVisible = true;
                            indicator.Text = "Dersler güncelleniyor..";
                            ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                            client.GetCurrentPeriodAsync(currentPeriod, Database.Student.ID, Database.Student.Password);
                            client.GetCurrentPeriodCompleted += client_GetCurrentPeriodCompleted;
                            SystemTray.SetProgressIndicator(this, indicator);
                        }
                        catch (Exception)
                        {
                            if (SystemTray.ProgressIndicator != null)
                                SystemTray.ProgressIndicator.IsVisible = false;
                        }
                    }
                }
                else
                    Container.DataContext = Database.Student.Programs[0].Periods.SingleOrDefault(x => x.Code == code && x.YearCode == yearCode);
                Lessons.SelectedItem = null;
            }
            base.OnNavigatedTo(e);
        }

        private void client_GetCurrentPeriodCompleted(object sender, GetCurrentPeriodCompletedEventArgs e)
        {
            if (SystemTray.ProgressIndicator != null)
                SystemTray.ProgressIndicator.IsVisible = false; 
            Connection con = e.Result;
            if (con.Message != null)
                MessageBox.Show(con.Message);
            if (con.State)
            {
                Period period = con.DataObject as Period;
                string text = "";
                foreach (Lesson newLesson in period.Lessons)
                {
                    Lesson oldLesson = Database.Student.Programs[0].Periods.First().Lessons.SingleOrDefault(x => x.Code == newLesson.Code);
                    bool isNew = false;
                    text += newLesson.Name + "\n";
                    if (newLesson.FirstMidterm.Mark != oldLesson.FirstMidterm.Mark && newLesson.FirstMidterm.Mark != null)
                    {
                        text += "  1. Vize: " + newLesson.FirstMidterm.Mark + "\n";
                        isNew = true;
                    }
                    if (newLesson.SecondMidterm.Mark != oldLesson.SecondMidterm.Mark && newLesson.SecondMidterm.Mark != null)
                    {
                        text += "  2. Vize: " + newLesson.SecondMidterm.Mark + "\n";
                        isNew = true;
                    }
                    if (newLesson.ThirdMidterm.Mark != oldLesson.ThirdMidterm.Mark && newLesson.ThirdMidterm.Mark != null)
                    {
                        text += "  3. Vize: " + newLesson.ThirdMidterm.Mark + "\n";
                        isNew = true;
                    }
                    if (newLesson.Final.Mark != oldLesson.Final.Mark && newLesson.Final.Mark != null)
                    {
                        text += "  Final: " + newLesson.Final.Mark + "\n";
                        isNew = true;
                    }
                    if (newLesson.Integration.Mark != oldLesson.Integration.Mark && newLesson.Integration.Mark != null)
                    {
                        text += "  Bütünleme: " + newLesson.Integration.Mark + "\n";
                        isNew = true;
                    }
                    if (newLesson.Average != oldLesson.Average && newLesson.Average != null)
                    {
                        text += "  Ortalama: " + newLesson.Average + "\n";
                        isNew = true;
                    }
                    if (!isNew)
                        text = text.Replace(newLesson.Name + "\n", "");
                }
                if (!string.IsNullOrEmpty(text))
                    MessageBox.Show(text);
                Database.Student.Programs[0].Periods.RemoveAt(0);
                Database.Student.Programs[0].Periods.Insert(0, period);
                Database.UpdateStudent();
                Container.DataContext = period;
            }
        }

        private void Lessons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Lesson selected = Lessons.SelectedItem as Lesson;
            if (selected != null)
                NavigationService.Navigate(new Uri("/LessonDetailsPage.xaml?Code=" + selected.Code + "&PeriodCode=" + code + "&PeriodYearCode=" + yearCode, UriKind.Relative));
        }
    }
}