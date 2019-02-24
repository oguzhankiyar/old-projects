using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace OK.Mobisis.Converters
{
    public class DayToStringConverter : IValueConverter
    {
        public string NullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var day = value as DayOfWeek?;

            switch (day)
            {
                case DayOfWeek.Monday:
                    return "Pazartesi";
                case DayOfWeek.Tuesday:
                    return "Salı";
                case DayOfWeek.Wednesday:
                    return "Çarşamba";
                case DayOfWeek.Thursday:
                    return "Perşembe";
                case DayOfWeek.Friday:
                    return "Cuma";
                case DayOfWeek.Saturday:
                    return "Cumartesi";
                case DayOfWeek.Sunday:
                    return "Pazar";
                default:
                    return NullValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
