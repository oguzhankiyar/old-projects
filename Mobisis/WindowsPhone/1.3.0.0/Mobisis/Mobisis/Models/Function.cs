using Microsoft.Phone.Scheduler;
using Mobisis.ObisisServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobisis.Models
{
    public class Function
    {
        public static Lesson CloneLesson(Lesson lesson)
        {
            return new Lesson()
            {
                Code = lesson.Code,
                Name = lesson.Name,
                FirstMidterm = new Exam() { Mark = lesson.FirstMidterm.Mark, Date = lesson.FirstMidterm.Date },
                SecondMidterm = new Exam() { Mark = lesson.SecondMidterm.Mark, Date = lesson.SecondMidterm.Date },
                ThirdMidterm = new Exam() { Mark = lesson.ThirdMidterm.Mark, Date = lesson.ThirdMidterm.Date },
                Final = new Exam() { Mark = lesson.Final.Mark, Date = lesson.Final.Date },
                Integration = new Exam() { Mark = lesson.Integration.Mark, Date = lesson.Integration.Date },
                SuccessFactor = lesson.SuccessFactor,
                Grade = lesson.Grade,
                IntegrationRight = lesson.IntegrationRight,
                State = lesson.State,
                AbsentState = lesson.AbsentState,
                AverageEffect = lesson.AverageEffect,
                Credit = lesson.Credit
            };
        }
        public static void StartTask()
        {
            try
            {
                if (Database.Settings.NotificationState)
                {
                    EndTask();
                    var taskName = "Mobisis Task";
                    var periodicTask = new PeriodicTask(taskName) { Description = "some desc" };
                    ScheduledActionService.Add(periodicTask);
                    ScheduledActionService.LaunchForTest(taskName, TimeSpan.FromSeconds(30));
                }
            }
            catch (Exception) { }
        }
        public static void EndTask()
        {
            try
            {
                var taskName = "Mobisis Task";
                if (ScheduledActionService.Find(taskName) != null)
                {
                    ScheduledActionService.Remove(taskName);
                }
            }
            catch (Exception) { }
        }
    }
}
