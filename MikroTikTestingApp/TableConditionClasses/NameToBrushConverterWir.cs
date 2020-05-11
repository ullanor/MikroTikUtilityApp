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
            number = -76;//xd
            if (input.Length - 9 > 0)
                int.TryParse(input.Substring(0, input.Length - 9), out number);

            if (number >= -60 && number != 0)
                return Brushes.LightGreen;

            else if(number >= -75 && number != 0)
                return Brushes.Yellow;

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
