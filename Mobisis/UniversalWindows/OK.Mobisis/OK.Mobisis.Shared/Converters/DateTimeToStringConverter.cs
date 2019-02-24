using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace OK.Mobisis.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public string Format { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = value as DateTime?;
            if (date == null)
                return null;
            return ((DateTime)date).ToString(this.Format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
