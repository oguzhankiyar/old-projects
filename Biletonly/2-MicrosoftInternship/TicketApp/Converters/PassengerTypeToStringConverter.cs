using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Biletall.Helper.Enums;
using TicketApp.Models;

namespace TicketApp.Converters
{
    public class PassengerTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PassengerType passengerType;
            Enum.TryParse<PassengerType>(value.ToString(), out passengerType);
            return passengerType.GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
