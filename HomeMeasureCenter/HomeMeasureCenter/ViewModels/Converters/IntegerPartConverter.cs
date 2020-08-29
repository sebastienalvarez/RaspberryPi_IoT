using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace HomeMeasureCenter.ViewModels.Converters
{
    public class IntegerPartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is double)
            {
                double val = (double)value;
                if (!double.IsNaN(val))
                {
                    string valString = val.ToString("F1");
                    return valString.Substring(0, valString.Length - 2);
                }
            }
            else if(value is uint)
            {
                uint val = (uint)value;
                if (val != uint.MaxValue)
                {
                    return val.ToString();
                }
            }
            else if(value is byte)
            {
                return ((byte)value).ToString();
            }
            else if(value is ulong)
            {
                ulong val = (ulong)value;
                if (val != ulong.MaxValue)
                {
                    return val.ToString();
                }
            }

            if(parameter is int)
            {
                int dashNumber = (int)parameter;
                return new string('-', dashNumber);
            }

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
