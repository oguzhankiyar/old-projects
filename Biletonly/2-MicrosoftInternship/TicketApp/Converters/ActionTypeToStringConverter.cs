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
    public class ActionTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ActionType actionType;
            Enum.TryParse<ActionType>(value.ToString(), out actionType);
            return actionType.GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
