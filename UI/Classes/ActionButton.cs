using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProcHacker.UI.Classes
{
    class ActionButton : Button
    {
        public ActionButton(Image _icon) : base()
        {
            Content = _icon;
            Background = UITools.UIBrushes.Transparent;
            BorderThickness = new Thickness(0);
        }

        public ActionButton(string _imagePath) : base()
        {
            Content = new Image { Source = new BitmapImage (new Uri(_imagePath, UriKind.Relative)) };
            Background = UITools.UIBrushes.Transparent;
            BorderThickness = new Thickness(0);
        }
    }
}
