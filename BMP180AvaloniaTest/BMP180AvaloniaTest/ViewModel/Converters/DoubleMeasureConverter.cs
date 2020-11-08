using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BMP180AvaloniaTest.ViewModel.Converters
{
    public class DoubleMeasureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertedValue = string.Empty;
            string unit = parameter != null ? parameter.ToString() : string.Empty;
            if (!unit.StartsWith("°"))
            {
                unit = " " + unit;
            }
            double measureValue = double.NaN;
            if(value.GetType() == typeof(double))
            {
                measureValue = (double)value;
                if(double.IsNaN(measureValue))
                {
                    convertedValue = "-";
                }
                else
                {
                    convertedValue = measureValue.ToString("f1") + parameter.ToString();
                }
            }
            return convertedValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
