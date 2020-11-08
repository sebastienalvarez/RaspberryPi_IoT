using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BMP180AvaloniaTest.ViewModel.Converters
{
    class BoolToConnectionMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertedMessage = string.Empty;
            if(value.GetType() == typeof(bool))
            {
                if ((bool)value)
                {
                    convertedMessage = "OK";
                }
                else
                {
                    convertedMessage = "KO";
                }
            }
            return convertedMessage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
