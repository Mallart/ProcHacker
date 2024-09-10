using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ProcHacker.UI.Classes
{
    public static class UITools
    {
        public static class UIBrushes
        {
            /// <summary>Transparent SolidColorBrush.</summary>
            public static SolidColorBrush Transparent = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        public static class Dictionaries
        {
            const string ThemesPath = "/UI";

            public static List<string> Themes = new List<string>
            {
                $"{ThemesPath}/DeepSea.xaml",
                $"{ThemesPath}/Reddish.xaml",
                $"{ThemesPath}/Global warning.xaml",
                $"{ThemesPath}/Dark.xaml",
            };
        }

        /// <summary>
        /// Changes the current color scheme of the app.
        /// </summary>
        public static void ChangeTheme(int _themeIndex, bool _appHasJustStarted = true)
        {
            GlobalSettings.currentTheme = _themeIndex % UITools.Dictionaries.Themes.Count;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(UITools.Dictionaries.Themes[GlobalSettings.currentTheme], UriKind.Relative) });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/UI/StaticColors.xaml", UriKind.Relative) });
            if (_appHasJustStarted)
            {
                MainWindow _nMainWindow = new MainWindow();
                MainWindow _oldWindow = (MainWindow)Application.Current.MainWindow;
                Application.Current.MainWindow = _nMainWindow;
                _nMainWindow.Show();
                _oldWindow.Close();
            }
            GC.Collect();
        }
    }
}
