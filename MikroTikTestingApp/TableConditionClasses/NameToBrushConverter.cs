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
    class NameToBrushConverter : IValueConverter
    {
        string input = string.Empty;
        int number = 0;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            input = value as string;
            number = 0;
            if (input.Length - 13 > 0)
                int.TryParse(input.Substring(0, input.Length - 13), out number);
            if (number >= 100)
                return Brushes.LightGreen;
            else
            {
                if (input.Length - 17 > 0)
                    int.TryParse(input.Substring(0, input.Length - 17), out number);
                //MessageBox.Show(number.ToString());
                if (number >= 100)
                    return Brushes.LawnGreen;
                else
                    return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
