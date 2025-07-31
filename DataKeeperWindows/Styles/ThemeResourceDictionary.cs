using System.Windows;

namespace DataKeeperWindows.Styles
{
    public class ThemeResourceDictionary : ResourceDictionary
    {
        private Uri? darkTheme;
        private Uri? lightTheme;

        public Uri? DarkTheme
        {
            get => darkTheme;
            set
            {
                darkTheme = value;
                UpdateTheme();
            }
        }

        public Uri? LightTheme
        {
            get => lightTheme;
            set
            {
                lightTheme = value;
                UpdateTheme();
            }
        }

        public void UpdateTheme()
        {
            Uri? uri = App.Theme == Themes.Dark ? darkTheme : lightTheme;

            if (uri != null && Source != uri)
            {
                Source = uri;
            }
        }
    }
}
