using DataKeeperWindows.Classes;
using System.Windows;
using System.Windows.Input;

namespace DataKeeperWindows.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateOrOpenFileWindow.xaml
    /// </summary>
    public partial class CreateOrOpenFileWindow : Window
    {
        private SturtupActions actions = SturtupActions.CreateFile;

        public CreateOrOpenFileWindow()
        {
            InitializeComponent();
        }
        internal SturtupActions GetSturtupActions() => actions;
        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            actions = SturtupActions.OpenFile;
            DialogResult = true;
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            actions = SturtupActions.CreateFile;
            DialogResult = true;
        }
    }
}
