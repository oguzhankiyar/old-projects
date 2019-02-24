using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System;
using MobisisScheduledTaskAgent.ObisisServiceReference;
using System.Linq;

namespace MobisisScheduledTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        DataSaver<Student> StudentData = new DataSaver<Student>();
        Student student;
        Period currentPeriod;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background


            if (task is PeriodicTask)
            {
                //ToastMessage = "";
            }
            try
            {
                student = StudentData.LoadMyData("Student");
                currentPeriod = student.Programs[0].Periods[0];
                if (student != null && currentPeriod != null && student.ID != null && student.Password != null)
                {
                    ObisisMobileServiceClient client = new ObisisMobileServiceClient();
                    client.GetCurrentPeriodAsync(currentPeriod, student.ID, student.Password);
                    client.GetCurrentPeriodCompleted += client_GetCurrentPeriodCompleted;
                }
                else
                {
                    NotifyComplete();
                }
            }
            catch (Exception)
            {
                NotifyComplete();
            }
        }

        void client_GetCurrentPeriodCompleted(object sender, GetCurrentPeriodCompletedEventArgs e)
        {
            Connection con = e.Result;
            if (con.State)
            {
                Period period = con.DataObject as Period;
                foreach (Lesson newLesson in period.Lessons)
                {
                    string text = "";
                    Lesson oldLesson = currentPeriod.Lessons.SingleOrDefault(x => x.Code == newLesson.Code);
                    bool isNew = false;
                    if (newLesson.FirstMidterm.Mark != oldLesson.FirstMidterm.Mark && newLesson.FirstMidterm.Mark != null)
                    {
                        text += "1. Vize: " + newLesson.FirstMidterm.Mark + " ";
                        isNew = true;
                    }
                    if (newLesson.SecondMidterm.Mark != oldLesson.SecondMidterm.Mark && newLesson.SecondMidterm.Mark != null)
                    {
                        text += "2. Vize: " + newLesson.SecondMidterm.Mark + " ";
                        isNew = true;
                    }
                    if (newLesson.ThirdMidterm.Mark != oldLesson.ThirdMidterm.Mark && newLesson.ThirdMidterm.Mark != null)
                    {
                        text += "3. Vize: " + newLesson.ThirdMidterm.Mark + " ";
                        isNew = true;
                    }
                    if (newLesson.Final.Mark != oldLesson.Final.Mark && newLesson.Final.Mark != null)
                    {
                        text += "Final: " + newLesson.Final.Mark + " ";
                        isNew = true;
                    }
                    if (newLesson.Integration.Mark != oldLesson.Integration.Mark && newLesson.Integration.Mark != null)
                    {
                        text += "Bütünleme: " + newLesson.Integration.Mark + " ";
                        isNew = true;
                    }
                    if (newLesson.Average != oldLesson.Average && newLesson.Average != null)
                    {
                        text += "Ortalama: " + newLesson.Average + " ";
                        isNew = true;
                    }
                    if (!isNew)
                        text = text.Replace(newLesson.Name + " ", "");
                    if (!string.IsNullOrEmpty(text))
                    {
                        ShellToast Toast = new ShellToast();
                        Toast.Title = "Mobisis: " + newLesson.Name;
                        Toast.Content = text;
                        //Toast.NavigationUri = new Uri("/LessonDetailsPage.xaml?PeriodCode=" + currentPeriod.Code + "&PeriodYearCode=" + currentPeriod.YearCode + "&Code=" + newLesson.Code, UriKind.Relative);
                        Toast.Show();
                    }
                }
                student.Programs[0].Periods.RemoveAt(0);
                student.Programs[0].Periods.Insert(0, period);
                StudentData.SaveMyData(student, "Student");
            }
            NotifyComplete();
        }
    }
}