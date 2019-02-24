using Microsoft.Phone.Controls;
using Mobisis.ObisisServiceReference;
using Mobisis.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mobisis.Models
{
    public class CalculateGANO
    {
        private static CheckBox lblName { get; set; }
        private static PhoneTextBox tbMid_1 { get; set; }
        private static PhoneTextBox tbMid_2 { get; set; }
        private static PhoneTextBox tbMid_3 { get; set; }
        private static PhoneTextBox tbFinal { get; set; }
        private static PhoneTextBox tbIntegration { get; set; }
        public static List<Lesson> Lessons { get; set; }

        public static Grid GetLessonPanel(Lesson currentLesson)
        {
            Lesson lesson = Function.CloneLesson(currentLesson);
            Lessons.Add(lesson);
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
     
            InputScope scope = new InputScope();
            InputScopeName scopeName = new InputScopeName() { NameValue = InputScopeNameValue.Number };
            scope.Names.Add(scopeName);

            lblName = new CheckBox() { IsChecked = lesson.AverageEffect, Content = lesson.Name, FontSize = 25 };
            lblName.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(lblName, 0);
            Grid.SetColumnSpan(lblName, 5);
            lblName.Checked += lblName_SelectionChanged;
            lblName.Unchecked += lblName_SelectionChanged;
            
            tbMid_1 = new PhoneTextBox() { Text = lesson.FirstMidterm.Mark.ToString(), Hint = "V1", InputScope = scope, MaxLength = 3, TextAlignment = TextAlignment.Center };
            tbMid_1.IsEnabled = lesson.FirstMidterm.Mark == null;
            Grid.SetRow(tbMid_1, 1);
            Grid.SetColumn(tbMid_1, 0);
            tbMid_1.TextChanged += tbMid_1_TextChanged;

            tbMid_2 = new PhoneTextBox() { Text = lesson.SecondMidterm.Mark.ToString(), Hint = "V2", InputScope = scope, MaxLength = 3, TextAlignment = TextAlignment.Center };
            tbMid_2.IsEnabled = lesson.SecondMidterm.Mark == null;
            Grid.SetRow(tbMid_2, 1);
            Grid.SetColumn(tbMid_2, 1);
            tbMid_2.TextChanged += tbMid_2_TextChanged;

            tbMid_3 = new PhoneTextBox() { Text = lesson.ThirdMidterm.Mark.ToString(), Hint = "V3", InputScope = scope, MaxLength = 3, TextAlignment = TextAlignment.Center };
            tbMid_3.IsEnabled = lesson.ThirdMidterm.Mark == null;
            Grid.SetRow(tbMid_3, 1);
            Grid.SetColumn(tbMid_3, 2);
            tbMid_3.TextChanged += tbMid_3_TextChanged;

            tbFinal = new PhoneTextBox() { Text = lesson.Final.Mark.ToString(), Hint = "FNL", InputScope = scope, MaxLength = 3, TextAlignment = TextAlignment.Center };
            tbFinal.IsEnabled = lesson.Final.Mark == null;
            Grid.SetRow(tbFinal, 1);
            Grid.SetColumn(tbFinal, 3);
            tbFinal.TextChanged += tbFinal_TextChanged;

            tbIntegration = new PhoneTextBox() { Text = lesson.Integration.Mark.ToString(), Hint = "BÜT", InputScope = scope, MaxLength = 3, TextAlignment = TextAlignment.Center };
            tbIntegration.IsEnabled = lesson.Integration.Mark == null;
            Grid.SetRow(tbIntegration, 1);
            Grid.SetColumn(tbIntegration, 4);
            tbIntegration.TextChanged += tbIntegration_TextChanged;

            tbMid_1.Visibility = lesson.AverageEffect ? Visibility.Visible : Visibility.Collapsed;
            tbMid_2.Visibility = lesson.AverageEffect ? Visibility.Visible : Visibility.Collapsed;
            tbMid_3.Visibility = lesson.AverageEffect ? Visibility.Visible : Visibility.Collapsed;
            tbFinal.Visibility = lesson.AverageEffect ? Visibility.Visible : Visibility.Collapsed;
            tbIntegration.Visibility = lesson.AverageEffect ? Visibility.Visible : Visibility.Collapsed;

            grid.Children.Add(lblName);
            grid.Children.Add(tbMid_1);
            grid.Children.Add(tbMid_2);
            grid.Children.Add(tbMid_3);
            grid.Children.Add(tbFinal);
            grid.Children.Add(tbIntegration);

            return grid;
        }

        private static void tbMid_1_TextChanged(object sender, TextChangedEventArgs e)
        {
            PhoneTextBox tb = sender as PhoneTextBox;
            Grid grid = tb.Parent as Grid;
            string name = (grid.Children[0] as CheckBox).Content.ToString();
            int? mark = string.IsNullOrEmpty(tb.Text) ? (int?)null : Convert.ToInt32(tb.Text);
            Lessons.SingleOrDefault(x => x.Name == name).FirstMidterm.Mark = mark;
        }

        private static void tbMid_2_TextChanged(object sender, TextChangedEventArgs e)
        {
            PhoneTextBox tb = sender as PhoneTextBox;
            Grid grid = tb.Parent as Grid;
            string name = (grid.Children[0] as CheckBox).Content.ToString();
            int? mark = string.IsNullOrEmpty(tb.Text) ? (int?)null : Convert.ToInt32(tb.Text);
            Lessons.SingleOrDefault(x => x.Name == name).SecondMidterm.Mark = mark;
        }

        private static void tbMid_3_TextChanged(object sender, TextChangedEventArgs e)
        {
            PhoneTextBox tb = sender as PhoneTextBox;
            Grid grid = tb.Parent as Grid;
            string name = (grid.Children[0] as CheckBox).Content.ToString();
            int? mark = string.IsNullOrEmpty(tb.Text) ? (int?)null : Convert.ToInt32(tb.Text);
            Lessons.SingleOrDefault(x => x.Name == name).ThirdMidterm.Mark = mark;
        }

        private static void tbFinal_TextChanged(object sender, TextChangedEventArgs e)
        {
            PhoneTextBox tb = sender as PhoneTextBox;
            Grid grid = tb.Parent as Grid;
            string name = (grid.Children[0] as CheckBox).Content.ToString();
            int? mark = string.IsNullOrEmpty(tb.Text) ? (int?)null : Convert.ToInt32(tb.Text);
            Lessons.SingleOrDefault(x => x.Name == name).Final.Mark = mark;
        }

        private static void tbIntegration_TextChanged(object sender, TextChangedEventArgs e)
        {
            PhoneTextBox tb = sender as PhoneTextBox;
            Grid grid = tb.Parent as Grid;
            string name = (grid.Children[0] as CheckBox).Content.ToString();
            int? mark = string.IsNullOrEmpty(tb.Text) ? (int?)null : Convert.ToInt32(tb.Text);
            Lessons.SingleOrDefault(x => x.Name == name).Integration.Mark = mark;
        }

        private static void lblName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Grid grid = cb.Parent as Grid;
            string name = (grid.Children[0] as CheckBox).Content.ToString();
            Lessons.SingleOrDefault(x => x.Name == name).AverageEffect = cb.IsChecked == false ? false : true;
            tbMid_1 = grid.Children[1] as PhoneTextBox;
            tbMid_2 = grid.Children[2] as PhoneTextBox;
            tbMid_3 = grid.Children[3] as PhoneTextBox;
            tbFinal = grid.Children[4] as PhoneTextBox;
            tbIntegration = grid.Children[5] as PhoneTextBox;

            tbMid_1.Visibility = cb.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            tbMid_2.Visibility = cb.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            tbMid_3.Visibility = cb.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            tbFinal.Visibility = cb.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            tbIntegration.Visibility = cb.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public static double Calculate()
        {
            List<Lesson> allLessons = Lessons.Where(x => x.AverageEffect).ToList();
            List<Period> periods = Database.Student.Programs[0].Periods.ToList();
            for (int i = 1; i < periods.Count; i++)
            {
                Period period = periods[i];
                foreach (var lesson in period.Lessons.ToList())
                {
                    if (allLessons.Where(x => x.Name == lesson.Name).Count() == 0)
                    {
                        allLessons.Add(lesson);
                    }
                }
            }
            double sumAverage = 0;
            double sumCredit = 0;
            foreach (var lesson in allLessons.Where(x => x.AverageEffect))
            {
                int midterm = (int)Math.Ceiling((double)(lesson.FirstMidterm.Mark == null ? 0 : lesson.FirstMidterm.Mark));
                if (lesson.SecondMidterm.Mark != null)
                    midterm = (int)Math.Ceiling((double)(lesson.FirstMidterm.Mark + lesson.SecondMidterm.Mark / 2));
                if (lesson.ThirdMidterm.Mark != null)
                    midterm = (int)Math.Ceiling((double)(lesson.FirstMidterm.Mark + lesson.SecondMidterm.Mark + lesson.ThirdMidterm.Mark / 3));
                int final = (int)Math.Ceiling((double)(lesson.Final.Mark == null ? 0 : lesson.Final.Mark));
                if (lesson.Integration.Mark != null)
                    final = (int)Math.Ceiling((double)(lesson.Integration.Mark == null ? 0 : lesson.Integration.Mark));
                double average = midterm * Database.Settings.MidtermFactor / 100 + final * Database.Settings.FinalFactor / 100;
                if (lesson.Grade == null)
                    sumAverage += GetFactor(GetGrade(average)) * lesson.Credit;
                else
                    sumAverage += GetFactor(lesson.Grade) * lesson.Credit;
                sumCredit += lesson.Credit;
            }
            return Math.Ceiling((sumAverage * 100 / sumCredit) - 0.5) / 100;
        }

        private static double GetFactor(GradeType? grade)
        {
            switch (grade)
            {
                case GradeType.AA:
                    return 4.0;
                case GradeType.BA:
                    return 3.5;
                case GradeType.BB:
                    return 3.0;
                case GradeType.CB:
                    return 2.5;
                case GradeType.CC:
                    return 2.0;
                case GradeType.DC:
                    return 1.5;
                case GradeType.DD:
                    return 1.0;
                case GradeType.FD:
                    return 0.5;
                case GradeType.FF:
                    return 0.0;
                default:
                    return 0.0;
            }
        }

        private static GradeType? GetGrade(double average)
        {
            int up = (int)Math.Ceiling(average);
            if (up >= Database.Settings.AA_Limit)
                return GradeType.AA;
            else if (up >= Database.Settings.BA_Limit)
                return GradeType.BA;
            else if (up >= Database.Settings.BB_Limit)
                return GradeType.BB;
            else if (up >= Database.Settings.CB_Limit)
                return GradeType.CB;
            else if (up >= Database.Settings.CC_Limit)
                return GradeType.CC;
            else if (up >= Database.Settings.DC_Limit)
                return GradeType.DC;
            else if (up >= Database.Settings.DD_Limit)
                return GradeType.DD;
            else if (up >= Database.Settings.FD_Limit)
                return GradeType.FD;
            else
                return GradeType.FF;
        }
    }
}