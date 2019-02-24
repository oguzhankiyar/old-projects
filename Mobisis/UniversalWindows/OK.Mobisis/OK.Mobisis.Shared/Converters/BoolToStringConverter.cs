using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace OK.Mobisis.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value as bool? == true)
                return TrueValue;
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
