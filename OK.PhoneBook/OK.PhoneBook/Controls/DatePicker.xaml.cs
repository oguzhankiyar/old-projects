using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace OK.PhoneBook.Controls
{
    public partial class DatePicker : UserControl
    {
        public static DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePicker), new PropertyMetadata(DateTime.Now, OnSelectedDatePropertyChanged));
        private static void OnSelectedDatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = sender as DatePicker;
            var newDate = Convert.ToDateTime(e.NewValue);

            ctrl.cbDay.SelectedItem = newDate.Day.ToString();
            ctrl.cbMonth.SelectedItem = months[newDate.Month - 1];
            ctrl.cbYear.SelectedItem = newDate.Year.ToString();

            if (ctrl.SelectedDateChanged != null)
                ctrl.SelectedDateChanged(sender, null);
        }

        public DateTime? SelectedDate {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public event SelectionChangedEventHandler SelectedDateChanged;

        private static List<string> months = new List<string>();

        public DatePicker()
        {
            InitializeComponent();

            int currentDay = DateTime.Now.Day;
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            var days = new List<string>();
            for (int i = 1; i <= 31; i++)
            {
                days.Add(i.ToString());
            }

            months = new List<string>() { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            var years = new List<string>();
            for (int i = -5; i <= 5; i++)
            {
                years.Add((currentYear + i).ToString());
            }

            cbDay.ItemsSource = days;
            cbMonth.ItemsSource = months;
            cbYear.ItemsSource = years;
            
            SelectedDate = DateTime.Now;
        }

        private void cbDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDate.Value != null)
                SelectedDate = new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, Convert.ToInt32(cbDay.SelectedItem));
        }

        private void cbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDate.Value != null)
                SelectedDate = new DateTime(SelectedDate.Value.Year, months.IndexOf((string)cbMonth.SelectedItem) + 1, SelectedDate.Value.Day);
        }

        private void cbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedDate.Value != null)
                SelectedDate = new DateTime(Convert.ToInt32(cbYear.SelectedItem), SelectedDate.Value.Month, SelectedDate.Value.Day);
        }
    }
}
