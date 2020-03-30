using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MikroTikTestingApp
{
    class NameToBrushConverterRates : IValueConverter
    {
        string input = string.Empty;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            input = value as string;
            if (input != string.Empty && input.Length > 18)
                return Brushes.LightGreen;//Red
            else
                return Brushes.Red; //return DependencyProperty.UnsetValue;

            //    default:
            //        return DependencyProperty.UnsetValue;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
