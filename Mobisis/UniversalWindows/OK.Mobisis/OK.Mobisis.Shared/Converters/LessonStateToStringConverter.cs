using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.Enums;
using Windows.UI.Xaml.Data;

namespace OK.Mobisis.Converters
{
    public class LessonStateToStringConverter : IValueConverter
    {
        public string NullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var state = value as LessonState?;

            switch (state)
            {
                case LessonState.Passed:
                    return "Geçti";
                case LessonState.Failed:
                    return "Kaldı";
                case LessonState.Absent:
                    return "Devamsız";
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
