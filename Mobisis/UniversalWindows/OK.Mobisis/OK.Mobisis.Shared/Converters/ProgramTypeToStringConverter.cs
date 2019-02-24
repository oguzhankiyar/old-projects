using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.Enums;
using Windows.UI.Xaml.Data;

namespace OK.Mobisis.Converters
{
    public class ProgramTypeToStringConverter : IValueConverter
    {
        public string NullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var type = value as ProgramType?;

            switch (type)
            {
                case ProgramType.Major:
                    return "Anadal Programı";
                case ProgramType.Minor:
                    return "Yandal Programı";
                default:
                    return NullValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
