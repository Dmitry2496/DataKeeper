using System;
using System.Windows;

namespace DataKeeperWindows.Windows
{
    /// <summary>
    /// Логика взаимодействия для PleaseWaitWindow.xaml
    /// </summary>
    public partial class PleaseWaitWindow : Window, IDisposable
    {
        public PleaseWaitWindow(string message)
        {
            InitializeComponent();

            reason.Text = message;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }
    }
}
