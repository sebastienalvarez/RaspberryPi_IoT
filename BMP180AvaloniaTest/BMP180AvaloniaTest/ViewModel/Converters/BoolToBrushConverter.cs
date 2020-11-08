using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BMP180AvaloniaTest.ViewModel.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush convertedBrush = null;
            if (value.GetType() == typeof(bool))
            {
                if ((bool)value)
                {
                    convertedBrush = new SolidColorBrush(Colors.ForestGreen);
                }
                else
                {
                    convertedBrush = new SolidColorBrush(Colors.Red);
                }
            }
            return convertedBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
