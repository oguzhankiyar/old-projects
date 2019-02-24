using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace OK.Mobisis
{
    public static class Extensions
    {
        static Grid statusGrid;

        public static Lesson Clone(this Lesson lesson)
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

        public static List<Lesson> Clone(this List<Lesson> lessons)
        {
            var newLessons = new List<Lesson>();
            foreach (var lesson in lessons)
            {
                newLessons.Add(lesson.Clone());
            }
            return newLessons;
        }

        public static void ShowStatus(this Grid container, string message)
        {
            statusGrid = new Grid();
            statusGrid.Width = container.Width;
            statusGrid.Height = container.Height;
            statusGrid.Background = new SolidColorBrush(Color.FromArgb(0x50, 0x00, 0x00, 0x00));

            var textBlock = new TextBlock();
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            //textBlock.Margin = new Thickness(0, grid.Width / 2 - 20, 0, 0);
            textBlock.FontSize = 17.5;
            textBlock.Text = message;

            statusGrid.Children.Add(textBlock);
            Canvas.SetLeft(statusGrid, 350);
            container.Children.Add(statusGrid);
        }

        public static void HideStatus(this Grid container)
        {
            container.Children.Remove(statusGrid);
        }

        public static void UpdateLessonsMark(this Period period, List<Lesson> lessons, out string message)
        {
            message = "";
            foreach (var lesson in period.Lessons)
            {
                string markMessage = "";
                var newLesson = lessons.Find(x => x.Name == lesson.Name);

                if (newLesson == null)
                    return;

                if (lesson.FirstMidterm.Mark != newLesson.FirstMidterm.Mark && newLesson.FirstMidterm.Mark != null)
                    markMessage += "1. Vize: " + newLesson.FirstMidterm.Mark + ", ";
                lesson.FirstMidterm = newLesson.FirstMidterm;

                if (lesson.SecondMidterm.Mark != newLesson.SecondMidterm.Mark && newLesson.SecondMidterm.Mark != null)
                    markMessage += "2. Vize: " + newLesson.SecondMidterm.Mark + ", ";
                lesson.SecondMidterm = newLesson.SecondMidterm;

                if (lesson.ThirdMidterm.Mark != newLesson.ThirdMidterm.Mark && newLesson.ThirdMidterm.Mark != null)
                    markMessage += "3. Vize: " + newLesson.ThirdMidterm.Mark + ", ";
                lesson.ThirdMidterm = newLesson.ThirdMidterm;

                if (lesson.Final.Mark != newLesson.Final.Mark && newLesson.Final.Mark != null)
                    markMessage += "Final: " + newLesson.Final.Mark + ", ";
                lesson.Final = newLesson.Final;

                if (lesson.Integration.Mark != newLesson.Integration.Mark && newLesson.Integration.Mark != null)
                    markMessage += "Bütünleme: " + newLesson.Integration.Mark + ", ";
                lesson.Integration = newLesson.Integration;

                lesson.IntegrationRight = newLesson.IntegrationRight;

                markMessage = markMessage.Trim(' ').Trim(',');
                if (!string.IsNullOrEmpty(markMessage))
                    message +=  lesson.Name + ":\n" + markMessage + "\n";

            }
            message = message.Trim('\n');
        }
    }
}
