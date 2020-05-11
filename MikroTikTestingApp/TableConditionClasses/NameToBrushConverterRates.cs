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
        char[] testArray;
        int r = 0;
        string number = string.Empty;
        float result = 0;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            input = value as string;
            //if (input != string.Empty && input.Length > 18)
            //    return Brushes.LightGreen;//Red
            //else
            //    return Brushes.Red; //return DependencyProperty.UnsetValue;
            if (input.Length < 3)
                return Brushes.Red;
            //MessageBox.Show(input.Length.ToString());

            testArray = input.ToCharArray();
            r = Array.IndexOf(testArray, 'R');

             if(r - 8 < 0)
                return Brushes.Red;

            number = input.Substring(3, r - 8);
            float.TryParse(number/*.Replace('.', ',')*/, out result); //win 7 fix

            if(result >= 50)
                return Brushes.LightGreen;
            else
                return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
