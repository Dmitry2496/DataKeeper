using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataKeeperWindows.Windows
{
    /// <summary>
    /// Логика взаимодействия для RenameTableWindow.xaml
    /// </summary>
    public partial class RenameTableWindow : Window
    {
        public List<string> Names { get; set; } = []; //массив имеющихся таблиц имен
        private int index;
        public int Index
        {
            get => index;
            set
            {
                index = value;
                if (Names.Count > index)
                {
                    currentName.Text += Names[index];
                }
            }
        }


        public RenameTableWindow()
        {
            InitializeComponent();
        }



        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
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
                if (buttonYes.IsEnabled)
                {
                    DialogResult = true;
                }
            }
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void InputName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputName.Text == "")
            {
                warningText.Text = "!!! Пустое название";
                buttonYes.IsEnabled = false;
                return;
            }

            if (inputName.Text == Names[Index])
            {
                warningText.Text = "!!! Старое и новое названия совпадают";
                buttonYes.IsEnabled = false;
                return;
            }

            if ((from p in Names where p == inputName.Text select p).Any())
            {
                warningText.Text = "!!! Таблица с таким названием уже есть";
                buttonYes.IsEnabled = false;
                return;
            }

            warningText.Text = "";
            buttonYes.IsEnabled = true;
        }
    }
}
