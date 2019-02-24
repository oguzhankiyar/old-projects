using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Biletall.Helper.Enums;

namespace TicketApp.Converters
{
    public class SegmentTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SegmentType segmentType;
            Enum.TryParse<SegmentType>(value.ToString(), out segmentType);
            switch (segmentType)
            {
                case SegmentType.Nonstop:
                    return "Molasız";
                case SegmentType.Stop:
                    return "Molalı";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
