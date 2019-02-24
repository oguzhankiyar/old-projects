using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace iTunesMusic.Converters
{
    class StringFormatConverter : IValueConverter
    {
        string Type { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;
            if (parameter == null)
                return value;
            return string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
