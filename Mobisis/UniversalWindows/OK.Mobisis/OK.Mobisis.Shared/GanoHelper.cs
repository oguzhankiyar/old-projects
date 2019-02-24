using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Data;
using OK.Mobisis.Entities.Enums;
using OK.Mobisis.Entities.ModelEntities;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace OK.Mobisis
{
    public class MarkTextBox : TextBox
    {
        public MarkTextBox()
        {
            this.BorderThickness = new Thickness(0);
            this.Background = new SolidColorBrush(Color.FromArgb(0x25, 0x00, 0x00, 0x00));
            this.Margin = new Thickness(0, 0, 2.5, 0);
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.TextAlignment = TextAlignment.Center;
            this.FontSize = 20;
            this.FontWeight = FontWeights.ExtraLight;
            this.Foreground = new SolidColorBrush(Colors.White);
            this.MaxLength = 3;
            this.Height = 35;
            var scope = new InputScope();
            var scopeName = new InputScopeName() { NameValue = InputScopeNameValue.Number };
            scope.Names.Add(scopeName);
            this.InputScope = scope;
        }
    }

    public class GanoHelper
    {
        private List<Lesson> tempLessons;
        private List<Lesson> lessons;
        private List<TextBlock> averageItems;
        public OK.Mobisis.Entities.ModelEntities.Program Program;

        public GanoHelper()
        {
            lessons = new List<Lesson>();
            averageItems = new List<TextBlock>();
        }

        public UIElement GetLessonPanel(Lesson lesson)
        {
            lessons.Add(lesson);
            var lessonGrid = new Grid();
            lessonGrid.Margin = new Thickness(5);

            lessonGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            lessonGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            lessonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            lessonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            lessonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            lessonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            lessonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            var topGrid = new Grid();
            topGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            topGrid.SetValue(Grid.ColumnSpanProperty, 5);

            var cbName = new CheckBox();
            cbName.IsChecked = true;
            cbName.Content = lesson.Name;
            cbName.FontSize = 20;
            cbName.FontWeight = FontWeights.ExtraLight;
            cbName.Checked += (s, e) =>
            {
                lesson.AverageEffect = true;
            };
            cbName.Unchecked += (s, e) =>
            {
                lesson.AverageEffect = false;
            };
            
            var tbAverage = new TextBlock();
            tbAverage.SetValue(Grid.ColumnProperty, 2);
            tbAverage.FontSize = 17.5;
            tbAverage.TextAlignment = TextAlignment.Right;
            tbAverage.Margin = new Thickness(10);
            tbAverage.Tag = lesson.Code;
            averageItems.Add(tbAverage);

            var tbFirstMidterm = new MarkTextBox();
            tbFirstMidterm.SetValue(Grid.RowProperty, 1);
            tbFirstMidterm.PlaceholderText = "V1";
            if (lesson.FirstMidterm != null && lesson.FirstMidterm.Mark != null)
            {
                tbFirstMidterm.Text = lesson.FirstMidterm.Mark.ToString();
                tbFirstMidterm.IsEnabled = false;
            }
            tbFirstMidterm.TextChanged += (s, e) =>
            {
                int mark;
                if (Int32.TryParse(tbFirstMidterm.Text, out mark))
                    lesson.FirstMidterm.Mark = mark;
                else
                    lesson.FirstMidterm.Mark = null;
            };

            var tbSecondMidterm = new MarkTextBox();
            tbSecondMidterm.SetValue(Grid.ColumnProperty, 1);
            tbSecondMidterm.SetValue(Grid.RowProperty, 1);
            tbSecondMidterm.PlaceholderText = "V2";
            if (lesson.SecondMidterm != null && lesson.SecondMidterm.Mark != null)
            {
                tbSecondMidterm.Text = lesson.SecondMidterm.Mark.ToString();
                tbSecondMidterm.IsEnabled = false;
            }
            tbSecondMidterm.TextChanged += (s, e) =>
            {
                int mark;
                if (Int32.TryParse(tbSecondMidterm.Text, out mark))
                    lesson.SecondMidterm.Mark = mark;
                else
                    lesson.SecondMidterm.Mark = null;
            };

            var tbThirdMidterm = new MarkTextBox();
            tbThirdMidterm.SetValue(Grid.ColumnProperty, 2);
            tbThirdMidterm.SetValue(Grid.RowProperty, 1);
            tbThirdMidterm.PlaceholderText = "V3";
            if (lesson.ThirdMidterm != null && lesson.ThirdMidterm.Mark != null)
            {
                tbThirdMidterm.Text = lesson.ThirdMidterm.Mark.ToString();
                tbThirdMidterm.IsEnabled = false;
            }
            tbThirdMidterm.TextChanged += (s, e) =>
            {
                int mark;
                if (Int32.TryParse(tbThirdMidterm.Text, out mark))
                    lesson.ThirdMidterm.Mark = mark;
                else
                    lesson.ThirdMidterm.Mark = null;
            };

            var tbFinal = new MarkTextBox();
            tbFinal.SetValue(Grid.ColumnProperty, 3);
            tbFinal.SetValue(Grid.RowProperty, 1);
            tbFinal.PlaceholderText = "FNL";
            if (lesson.Final != null && lesson.Final.Mark != null)
            {
                tbFinal.Text = lesson.Final.Mark.ToString();
                tbFinal.IsEnabled = false;
            }
            tbFinal.TextChanged += (s, e) =>
            {
                int mark;
                if (Int32.TryParse(tbFinal.Text, out mark))
                    lesson.Final.Mark = mark;
                else
                    lesson.Final.Mark = null;
            };

            var tbIntegration = new MarkTextBox();
            tbIntegration.SetValue(Grid.ColumnProperty, 4);
            tbIntegration.SetValue(Grid.RowProperty, 1);
            tbIntegration.PlaceholderText = "BUT";
            if (lesson.Integration != null && lesson.Integration.Mark != null)
            {
                tbIntegration.Text = lesson.Integration.Mark.ToString();
                tbIntegration.IsEnabled = false;
            }
            tbIntegration.TextChanged += (s, e) =>
            {
                int mark;
                if (Int32.TryParse(tbIntegration.Text, out mark))
                    lesson.Integration.Mark = mark;
                else
                    lesson.Integration.Mark = null;
            };

            topGrid.Children.Add(cbName);
            topGrid.Children.Add(tbAverage);
            lessonGrid.Children.Add(topGrid);
            lessonGrid.Children.Add(tbFirstMidterm);
            lessonGrid.Children.Add(tbSecondMidterm);
            lessonGrid.Children.Add(tbThirdMidterm);
            lessonGrid.Children.Add(tbFinal);
            lessonGrid.Children.Add(tbIntegration);

            var outerGrid = new Grid();
            outerGrid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Colors.Black), Opacity = 0.3 });
            outerGrid.Children.Add(lessonGrid);
            outerGrid.Margin = new Thickness(5);

            return outerGrid;
        }

        public double Calculate()
        {
            tempLessons = new List<Lesson>();
            double totalAverage = 0, totalCredit = 0;
            int i = 0;
            foreach (var period in Program.Periods)
            {
                i++;
                foreach (var lesson in (i == 1 ? lessons : period.Lessons))
                {
                    if (tempLessons.Find(x => x.Code == lesson.Code) == null && lesson.AverageEffect == true)
                    {
                        double? factor;
                        if (i == 1)
                        {
                            int? average = GetAverage(lesson);
                            factor = GetAverageFactor(average);
                            var averageItem = averageItems.Find(x => x.Tag.ToString() == lesson.Code);
                            if (factor != null && average != null)
                                averageItem.Text = average.ToString() + " [" + GetGrade(average).ToString() + "]";
                            else
                                averageItem.Text = "";
                        }
                        else
                            factor = GetAverageFactor(lesson.Grade);

                        if (factor != null)
                        {
                            tempLessons.Add(lesson);
                            totalAverage += lesson.Credit * (double)factor;
                            totalCredit += lesson.Credit;
                        }
                    }
                }
            }

            if (totalCredit == 0)
                return 0;
            return totalAverage / totalCredit;
        }

        private int? GetAverage(Lesson lesson)
        {
            double average, midterm, final;

            if (lesson.FirstMidterm != null && lesson.FirstMidterm.Mark != null)
                midterm = (double)lesson.FirstMidterm.Mark;
            else
                return null;

            if (lesson.SecondMidterm != null && lesson.SecondMidterm.Mark != null)
                midterm = (double)((lesson.FirstMidterm.Mark + lesson.SecondMidterm.Mark) / 2);
            if (lesson.ThirdMidterm != null && lesson.ThirdMidterm.Mark != null)
                midterm = (double)((lesson.FirstMidterm.Mark + lesson.SecondMidterm.Mark + lesson.ThirdMidterm.Mark) / 3);

            if (lesson.Final != null && lesson.Final.Mark != null)
                final = (double)lesson.Final.Mark;
            else
                return null;

            if (lesson.Integration != null && lesson.Integration.Mark != null)
                final = (double)lesson.Integration.Mark;
            
            average = (midterm * Database.SavedData.Settings.MidtermFactor / 100) + (final * Database.SavedData.Settings.FinalFactor / 100);

            return (int)Math.Ceiling(average);
        }

        private double? GetAverageFactor(int? mark)
        {
            if (mark == null)
                return null;

            if (mark >= Database.SavedData.Settings.AA_Limit)
                return 4;
            else if (mark >= Database.SavedData.Settings.BA_Limit)
                return 3.5;
            else if (mark >= Database.SavedData.Settings.BB_Limit)
                return 3;
            else if (mark >= Database.SavedData.Settings.CB_Limit)
                return 2.5;
            else if (mark >= Database.SavedData.Settings.CC_Limit)
                return 2;
            else if (mark >= Database.SavedData.Settings.DC_Limit)
                return 1.5;
            else if (mark >= Database.SavedData.Settings.DD_Limit)
                return 1;
            else if (mark >= Database.SavedData.Settings.FD_Limit)
                return 0.5;
            else
                return 0.0;
        }
        
        private GradeType? GetGrade(int? mark)
        {
            if (mark == null)
                return null;

            int up = (int)Math.Ceiling((double)mark);
            if (up >= Database.SavedData.Settings.AA_Limit)
                return GradeType.AA;
            else if (up >= Database.SavedData.Settings.BA_Limit)
                return GradeType.BA;
            else if (up >= Database.SavedData.Settings.BB_Limit)
                return GradeType.BB;
            else if (up >= Database.SavedData.Settings.CB_Limit)
                return GradeType.CB;
            else if (up >= Database.SavedData.Settings.CC_Limit)
                return GradeType.CC;
            else if (up >= Database.SavedData.Settings.DC_Limit)
                return GradeType.DC;
            else if (up >= Database.SavedData.Settings.DD_Limit)
                return GradeType.DD;
            else if (up >= Database.SavedData.Settings.FD_Limit)
                return GradeType.FD;
            else
                return GradeType.FF;
        }

        private double? GetAverageFactor(GradeType? grade)
        {
            switch (grade)
            {
                case GradeType.FF:
                    return 0.0;
                case GradeType.FD:
                    return 0.5;
                case GradeType.DD:
                    return 1.0;
                case GradeType.DC:
                    return 1.5;
                case GradeType.CC:
                    return 2.0;
                case GradeType.CB:
                    return 2.5;
                case GradeType.BB:
                    return 3.0;
                case GradeType.BA:
                    return 3.5;
                case GradeType.AA:
                    return 4.0;
                default:
                    return null;
            }
        }
    }
}
