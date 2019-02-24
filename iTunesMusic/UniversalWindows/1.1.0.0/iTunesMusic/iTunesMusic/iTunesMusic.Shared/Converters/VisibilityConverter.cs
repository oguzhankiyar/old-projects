using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace iTunesMusic.Converters
{
    class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return "Collapsed";
            return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
