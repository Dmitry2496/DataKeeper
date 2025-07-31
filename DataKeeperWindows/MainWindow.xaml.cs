using DataKeeperWindows.Styles;
using DataKeeperWindows.ViewModels;
using DataKeeperWindows.Windows;
using Microsoft.Win32;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataKeeperWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string[] args)
        {
            InitializeComponent();

            // Чтение темы из реестра
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Application"))
            {
                int? readTheme = Convert.ToInt32(key.GetValue("CurrentTheme", Themes.Light));
                switch (readTheme)
                {
                    case (int)Themes.Dark:
                        (Application.Current as App)?.ChangeSkin(Themes.Dark);
                        theme1.IsChecked = false;
                        theme2.IsChecked = true;
                        break;
                    case (int)Themes.Light:
                        (Application.Current as App)?.ChangeSkin(Themes.Light);
                        theme1.IsChecked = true;
                        theme2.IsChecked = false;
                        break;
                    default:
                        (Application.Current as App)?.ChangeSkin(Themes.Light);
                        theme1.IsChecked = true;
                        theme2.IsChecked = false;
                        break;
                }
            }


            // Определение аргументов
            if (args == null || args.Length == 0)
            {
                CreateOrOpenFileWindow createOrOpenFileWindow = new();
                bool? createOrOpenWindowResult = createOrOpenFileWindow.ShowDialog();
                DataContext = createOrOpenWindowResult is null or false
                    ? new MainViewModel()
                    : new MainViewModel(createOrOpenFileWindow.GetSturtupActions());
            }
            else
            {
                DataContext = new MainViewModel(args[0]);
            }

            tabControl.SelectedIndex = -1;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Размер top-панели определялся исходя из следующей зависимости:
            // Width = "{Binding WidthWindow, ConverterParameter=-15, Converter={StaticResource WidthWindowConverter}, ElementName=window}"
            // При работе был найден косяк: при разворачивании окна, затем сворачивании, затем восстановлении размер верхней полоски не запоминался и вел себя некорректно
            // Поэтому был реализован данный костыль

            topCanvas.Width = window.Width - 6;
            topPanel.Width = window.Width - 6;
            bottomCanvas.Width = window.Width - 6;
            bottomPanel.Width = window.Width - 6;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F || e.SystemKey == Key.F)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {
                    menuShowSearch.IsChecked = !menuShowSearch.IsChecked;
                    if (DataContext is MainViewModel mainViewModel && mainViewModel!.OpenFileCorrect)
                    {
                        if (menuShowSearch.IsChecked)
                        {
                            bottomCanvas.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            bottomCanvas.Visibility = Visibility.Collapsed;
                            DumpSearch();
                            tabControl.Focus();
                        }
                    }
                }
            }
        }

        #region TopPanel
        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
#pragma warning disable IDE0003
            this.DragMove();

            /* 
             * Если нужно, чтобы окно не выходило за края 
             if (this.Top <= 0)
             {
                 this.Top = 0;
             }

             if (this.Top >= (SystemParameters.PrimaryScreenHeight - this.Height))
             {
                 this.Top = SystemParameters.PrimaryScreenHeight - this.Height;
             }

             if (this.Left <= 0)
             {
                 this.Left = 0;
             }

             if (this.Left >= (SystemParameters.PrimaryScreenWidth - this.Width))
             {
                 this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
             }*/
#pragma warning restore IDE0003
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable IDE0003
            this.WindowState = WindowState.Minimized;
#pragma warning restore IDE0003

        }
        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable IDE0003
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                butMaximize.Style = (Style)butMaximize.FindResource("TopButton.Maximize.Style");
            }
            else if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                butMaximize.Style = (Style)butMaximize.FindResource("TopButton.Restore.Style");
            }
#pragma warning restore IDE0003
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable IDE0003
            this.Close();
#pragma warning restore IDE0003
        }
        #endregion



        #region Menu
        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Theme1_Click(object sender, RoutedEventArgs e)
        {
            if (theme1.IsChecked)
            {
                (Application.Current as App)?.ChangeSkin(Themes.Light);
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Application"))
                {
                    key.SetValue("CurrentTheme", (int)Themes.Light);
                }
                theme2.IsChecked = false;
            }
            else
            {
                theme1.IsChecked = true;
            }
        }
        private void Theme2_Click(object sender, RoutedEventArgs e)
        {
            if (theme2.IsChecked)
            {
                (Application.Current as App)?.ChangeSkin(Themes.Dark);
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}\Application"))
                {
                    key.SetValue("CurrentTheme", (int)Themes.Dark);
                }
                theme1.IsChecked = false;
            }
            else
            {
                theme2.IsChecked = true;
            }
        }

        private void ShowSearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel && mainViewModel!.OpenFileCorrect)
            {
                if ((sender as MenuItem)!.IsChecked)
                {
                    bottomCanvas.Visibility = Visibility.Visible;
                }
                else
                {
                    bottomCanvas.Visibility = Visibility.Collapsed;
                    DumpSearch();
                    tabControl.Focus();
                }
            }
        }
        #endregion


        #region Search
        private void TbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is MainViewModel dataContext)
                {
                    dataContext!.SearchCommand.Execute((tabControl.SelectedIndex, tbSearch.Text, chbSearch.IsChecked ?? false));
                }
            }
        }
        private void BtnDump_Click(object sender, RoutedEventArgs e)
        {
            DumpSearch();
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel dataContext)
            {
                dataContext!.SearchCommand.Execute((tabControl.SelectedIndex, tbSearch.Text, chbSearch.IsChecked ?? false));
            }
        }
        private void DumpSearch()
        {
            if (DataContext is MainViewModel dataContext)
            {
                dataContext!.SearchCommand.Execute(tabControl.SelectedIndex);
            }
        }
        #endregion



        #region DataGrid
        private void Data_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid && 0 < dataGrid.Columns.Count)
            {
                dataGrid.Columns[0].IsReadOnly = true;
            }
        }

        private void Data_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Delete || e.SystemKey == Key.Delete) && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                if (DataContext is MainViewModel mainViewModel && mainViewModel!.OpenFileCorrect &&
                    sender is DataGrid dataGrid && 0 <= dataGrid.SelectedItems.Count)
                {
                    // подтверждение
                    if (new MessageBoxWindow($"Удалить выбранные строки?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning).ShowDialog() != true)
                    {
                        return;
                    }

                    // Удаление
                    if (dataGrid.SelectedItems.Count == 1)
                    {
                        mainViewModel!.RemoveRowCommand.Execute((tabControl.SelectedIndex, dataGrid.SelectedIndex));
                    }
                    else
                    {
                        List<int> rowsIndex = [];
                        for (int i = 0; i < dataGrid.SelectedItems.Count; i++)
                        {
                            DataRowView? dataRowView = (DataRowView?)dataGrid.SelectedItems[i];
                            string idSelectRow = string.Empty;
                            if (dataRowView is not null)
                            {
                                idSelectRow = (string)dataRowView[0];
                            }

                            for (int j = 0; j < mainViewModel.TablesView[tabControl.SelectedIndex].Table.Rows.Count; j++)
                            {
                                string idRow = (string)mainViewModel.TablesView[tabControl.SelectedIndex].Table.Rows[j][0];
                                if (idSelectRow == idRow)
                                {
                                    rowsIndex.Add(j);
                                    break;
                                }
                            }
                        }

                        mainViewModel!.RemoveRowCommand.Execute((tabControl.SelectedIndex, rowsIndex));
                    }
                }
            }
        }

        private void Data_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit &&
                DataContext is MainViewModel mainViewModel && mainViewModel!.OpenFileCorrect &&
                sender is DataGrid dataGrid)
            {
                mainViewModel!.ChangeCellCommand.Execute(
                    (tabControl.SelectedIndex, dataGrid.SelectedIndex, e.Column.DisplayIndex, (e.EditingElement as TextBox)!.Text));
            }
        }
        #endregion
    }
}