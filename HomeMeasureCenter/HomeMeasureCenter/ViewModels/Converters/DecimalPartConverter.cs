using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace HomeMeasureCenter.ViewModels.Converters
{
    public class DecimalPartConverter : IValueConverter
    {
        private static NumberFormatInfo nfi = new CultureInfo("fr-FR").NumberFormat;

        public DecimalPartConverter()
        {
            nfi.NumberDecimalSeparator = ".";
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is double)
            {
                double val = (double)value;
                if (!double.IsNaN(val))
                {
                    string valString = val.ToString("F1", nfi);
                    return valString.Substring(valString.Length - 2);
                }
            }
            return ".-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
