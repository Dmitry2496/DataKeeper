using System.Windows;
using System.Windows.Input;

namespace DataKeeperWindows.Windows
{
    // Возможные возвращаемые значения:
    // "ОК", "ДА", "Enter" - true
    // "Нет", "Закрыть" - false
    // "Отмена", "Escape" - null
    /// <summary>
    /// Логика взаимодействия для MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        public byte ImageType { get; set; }

        public MessageBoxWindow(string message, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.None)
        {
            ImageType = (byte)image;

            InitializeComponent();

            tittle.Content = caption;
            this.message.Text = message;

            switch (button)
            {
                case MessageBoxButton.OK:
                    buttonYes.Content = "OK";
                    buttonNo.Visibility = Visibility.Collapsed;
                    buttonCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.OKCancel:
                    buttonYes.Content = "OK";
                    buttonNo.Visibility = Visibility.Collapsed;
                    buttonCancel.Content = "Отмена";
                    break;
                case MessageBoxButton.YesNoCancel:
                    buttonYes.Content = "Да";
                    buttonNo.Content = "Нет";
                    buttonCancel.Content = "Отмена";
                    break;
                case MessageBoxButton.YesNo:
                    buttonYes.Content = "Да";
                    buttonNo.Content = "Нет";
                    buttonCancel.Visibility = Visibility.Collapsed;
                    break;
                default:
                    buttonYes.Content = "OK";
                    buttonNo.Visibility = Visibility.Collapsed;
                    buttonCancel.Visibility = Visibility.Collapsed;
                    break;
            }
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
        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = null;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = null;
            }
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
            }
        }
    }
}
