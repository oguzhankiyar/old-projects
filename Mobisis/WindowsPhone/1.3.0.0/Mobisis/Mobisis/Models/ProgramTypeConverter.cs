using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Mobisis.ObisisServiceReference;

namespace Mobisis.Models
{
    public class ProgramTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return ("Anadal Programı");
            else
                return ((ProgramType)value == ProgramType.Minor ? "Yandal Programı" : "Anadal Programı"); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals("Yandal Programı") ? ProgramType.Minor : ProgramType.Major : ProgramType.Major;
        }
    }
}
