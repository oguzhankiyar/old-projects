using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using OK.Mobisis.Api.Helper;
using OK.Mobisis.Api.Helper.Enums;
using OK.Mobisis.Api.Helper.Parsings;
using OK.Mobisis.Api.Helper.Requests;
using OK.Mobisis.Data;
using OK.Mobisis.Entities.ModelEntities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OK.Mobisis.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LessonListPage : Page
    {
        private Period tempPeriod;

        public LessonListPage()
        {
            this.InitializeComponent();
            new SwipeMenu().AddTo(this);

            Prepare();
        }

        private void Prepare()
        {
            if (Database.TempData.Student == null)
            {
                this.LayoutRoot.Children.Add(new LoginRequiredControl());
            }
            else
            {
                if (Database.TempData.Student == Database.SavedData.Student)
                    Database.TempData.Student = Database.SavedData.Student;

                var period = Database.TempData.Period;
                if (period == null)
                {
                    period = Database.TempData.Student.Programs[0].Periods.FirstOrDefault();
                    if (period == null)
                    {
                        this.LayoutRoot.Children.Add(new MessageControl("Henüz ders almıyorsunuz.."));
                        return;
                    }
                }
                else
                    btnRefresh.Visibility = Visibility.Collapsed;

                tempPeriod = period;

                this.tbPeriodName.Text = period.Year + " " + period.Name;
                this.LessonList.ItemsSource = period.Lessons;
                Database.TempData.Period = null;
            }
        }

        private async void LessonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lesson = LessonList.SelectedItem as Lesson;
            if (lesson != null)
            {
                Database.TempData.Lesson = lesson;
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.Frame.Navigate(typeof(LessonDetailsPage));
                });
            }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                await new MessageDialog("İnternet bağlantınızı kontrol ediniz").ShowAsync();
            }
            else
            {
                this.LayoutRoot.ShowStatus("dersler güncelleniyor..");
                LessonRequests.LessonRequest.OnCompleted += GetLessons_Completed;
                LessonRequests.GetLessons(Database.TempData.Student.Id, Database.TempData.Student.Password, tempPeriod.Code, tempPeriod.YearCode);
            }
        }

        private async void GetLessons_Completed(BaseResponse<List<Lesson>> lessonResponse)
        {
            this.LayoutRoot.HideStatus();
            if (lessonResponse.Status != ResponseStatus.Successful)
            {
                await new MessageDialog("Dersler güncellenemedi").ShowAsync();
            }
            else
            {
                var lessons =  lessonResponse.Result as List<Lesson>;
                string message;
                tempPeriod.UpdateLessonsMark(lessons, out message);
                foreach (var program in Database.TempData.Student.Programs)
	            {
                    var period = program.Periods.FirstOrDefault(x => x.Code == tempPeriod.Code && x.YearCode == tempPeriod.YearCode);
                    if (period != null)
                        period.Lessons = tempPeriod.Lessons;
	            }
                if (Database.TempData.RememberMe)
                {
                    Database.SavedData.Student = Database.TempData.Student;
                    await Database.Update();
                }
                this.LessonList.ItemsSource = null;
                this.LessonList.ItemsSource = tempPeriod.Lessons;
                if (!string.IsNullOrEmpty(message))
                {
                    await new MessageDialog(message).ShowAsync();
                }
            }
        }
    }
}
