using System.Windows;
using System.Windows.Input;

namespace DataKeeperWindows.Windows
{
    /// <summary>
    /// Логика взаимодействия для InputPasswordWindow.xaml
    /// </summary>
    public partial class InputPasswordWindow : Window
    {
        public InputPasswordWindow()
        {
            InitializeComponent();

            password.Focus();
        }
        
        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
            }
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
            }
        }
    }
}
