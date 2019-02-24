using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OK.CargoTracking.Windows.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;
            var date = (DateTime)value;
            if (date != null)
            {
                if (date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                    return date.Day + " " + GetMonthName(date.Month) + " " + date.ToString("yyyy");
                else
                    return date.Day + " " + GetMonthName(date.Month) + " " + date.ToString("yyyy, HH:mm");
            }
            return value.ToString();
        }

        private string GetMonthName(int i)
        {
            string[] monthNames = new string[12] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            return monthNames[i - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
