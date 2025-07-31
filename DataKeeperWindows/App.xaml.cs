using DataKeeperWindows.Styles;
using System.Windows;

namespace DataKeeperWindows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Метод, который взывается при запуске приложения
        /// </summary>
        /// <param name="e"></param>
        /// Метод переопределяется для возможности запуска приложения с аргументами (например открытие файла)
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new(e.Args);
            window.Show();
        }

        #region ChangeThemeApp
        public static Themes Theme { get; set; } = Themes.Light;
        public void ChangeSkin(Themes newTheme)
        {
            Theme = newTheme;
            foreach (ResourceDictionary dict in Resources.MergedDictionaries)
            {
                if (dict is ThemeResourceDictionary themeDict)
                {
                    themeDict.UpdateTheme();
                }
            }
        }
        #endregion
    }
}
