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
    class NameToBrushConverterWir : IValueConverter
    {
        string input = string.Empty;
        int number = 0;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            input = value as string;
            number = 0;
            if (input.Length - 12 > 0)
                int.TryParse(input.Substring(0, input.Length - 12), out number);
            if (number > 10)
                return Brushes.Blue;//Red
            else
                return Brushes.LightGreen; //return DependencyProperty.UnsetValue;

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
