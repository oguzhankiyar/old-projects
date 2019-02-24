using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TicketApp.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var date = System.Convert.ToDateTime(value);
            string val = string.Empty;
            if (date.Hour != 0)
                val += date.Hour + " saat ";
            if (date.Minute != 0)
                val += date.Minute + " dakika ";
            if (date.Second != 0)
                val += date.Second + " saniye";
            val = val.Trim();
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
