using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataKeeperWindows.Classes
{
    internal class RowToIndexConverter : IValueConverter
    {
        /// <summary>
        /// Класс для отображения номеров строк в DataGrid
        /// </summary>
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            var item = (DataGridRow)value;
            var datgrid = ItemsControl.ItemsControlFromItemContainer(item) as DataGrid;
            int index = datgrid!.ItemContainerGenerator.IndexFromContainer(item) + 1;
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
