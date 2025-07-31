// Конвертер ширины верхней панели окна (уменьшение, чтобы она входила в окно) (работает только в привязке XAML)
using System;
using System.Globalization;
using System.Windows.Data;

namespace DataKeeperWindows.Classes
{
    public class WidthWindowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (double)value + double.Parse(parameter.ToString()!);
            }
            catch { return (double)value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
