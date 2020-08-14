/****************************************************************************************************************************************
 * 
 * Classe BoolToVisibilityConverter
 * Auteur : S. ALVAREZ
 * Date : 09-08-2020
 * Statut : En test
 * Version : 1
 * Revisions : NA
 *  
 * Objet : Classe de conversion commune, une valeur de type bool est convertir en une valeur de type Visibility.
 * 
 ****************************************************************************************************************************************/

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IoTUtilities.ViewModel.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        // Configure le "sens" de conversion
        public bool Invert { get; set; } = false;

        // Convertit bool => Visibility
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is bool)
            {
                bool transformedValue;
                if (Invert)
                {
                    transformedValue = !(bool)value;
                }
                else
                {
                    transformedValue = (bool)value;
                }
                return transformedValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Invert ? Visibility.Visible : Visibility.Collapsed;
        }

        // Convertit Visibility => bool
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if(value is Visibility)
            {
                bool convertedValue = (Visibility)value == Visibility.Visible ? true : false;
                if (Invert)
                {
                    return !convertedValue;
                }
                else
                {
                    return convertedValue;
                }
            }
            return Invert;
        }

    }
}
