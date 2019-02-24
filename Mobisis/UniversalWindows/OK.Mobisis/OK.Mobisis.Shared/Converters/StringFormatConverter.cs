using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace OK.Mobisis.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public string Format { get; set; }
        public string NullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return NullValue;

            if (string.IsNullOrEmpty(this.Format))
                return value.ToString();

            return string.Format(Format, value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
