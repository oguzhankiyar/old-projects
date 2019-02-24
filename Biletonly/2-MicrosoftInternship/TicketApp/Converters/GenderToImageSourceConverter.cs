using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Biletall.Helper.Enums;

namespace TicketApp.Converters
{
    public class GenderToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            Gender gender;
            Enum.TryParse<Gender>(value.ToString(), out gender);
            if (gender == Gender.Male)
                return "/Assets/male.png";
            else
                return "/Assets/female.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
