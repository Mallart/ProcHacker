using ProcHacker.FakeData;
using ProcHacker.Registry;
using ProcHacker.UI.Classes;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProcHacker.Tabs
{
    public static class General
    {
        private static TextBox _DevicesLibrary_ProcessorName;
        private static ListBox _DevicesLibrary_ProcessorsListBox;

        /// <summary>
        /// Sets Row and Column of an element in its parent grid.
        /// </summary>
        /// <param name="_element">Element to set row and column.</param>
        /// <param name="_row">Grid's row</param>
        /// <param name="_column">Grid's column</param>
        public static void SetRowCol(this UIElement _element, int _row, int _column)
        {
            Grid.SetRow(_element, _row);
            Grid.SetColumn(_element, _column);
        }

        private static void RefreshDevicesList(this ListBox _listbox)
        {
            LibManager.UpdateList();
            _listbox.Items.Clear();
            foreach (Processor _proc in LibManager.Processors)
                _listbox.Items.Add(new ListBoxItem { Content = _proc.Name });
        }

        private static void EditCPUName(object _sender, RoutedEventArgs e)
        {
            if (_DevicesLibrary_ProcessorsListBox.SelectedItem != null)
            {
                if (RegistryManager.OverWriteNoPS(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString", ((ListBoxItem)_DevicesLibrary_ProcessorsListBox.SelectedItem).Content.ToString())))
                    MessageBox.Show("Processor name successfully modified !", "ProcHacker", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Error while modifying the key", "ProcHacker", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                MessageBox.Show("An error occured while modyfying the key.", "ProcHacker", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void DevicesLibrary(ref Grid _container)
        {
            Grid _buttonSplitter = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            _buttonSplitter.SetRowCol(0, 0);

            ListBox _devices = new ListBox
            {
                Background = (SolidColorBrush)MainWindow.GetResource("NavPanelOverButton"),
                BorderBrush = UITools.UIBrushes.Transparent,
                Foreground = (SolidColorBrush)MainWindow.GetResource("ActiveText1"),
            };
            _devices.MouseDoubleClick += (object _sender, System.Windows.Input.MouseButtonEventArgs _e) =>
            { LibManager.Remove(_devices.SelectedIndex); _devices.RefreshDevicesList(); };
            _devices.SetRowCol(2, 0);

            TextBox _inputName = new TextBox
            {
                Height = 30,
                Width = 400,
                Foreground = (SolidColorBrush)MainWindow.GetResource("Title1"),
                Background = (SolidColorBrush)MainWindow.GetResource("NavPanelActiveButton"),
                BorderBrush = UITools.UIBrushes.Transparent,
                CaretBrush = (SolidColorBrush)MainWindow.GetResource("Text1"),
                FontFamily = new FontFamily("Cascadia Code SemiBold"),
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Name = "_inputProc",
                Margin = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _inputName.SetRowCol(1, 0);

            ActionButton _removeDevice = new ActionButton("UI/Assets/RemoveDevice.png");
            _removeDevice.Click += (object _sender, RoutedEventArgs e) => { if (!LibManager.Remove(_devices.SelectedIndex)) LibManager.Remove(_DevicesLibrary_ProcessorName.Text); _devices.RefreshDevicesList(); };
            ActionButton _addDevice = new ActionButton("UI/Assets/AddDevice.png");
            _addDevice.Click += (object _sender, RoutedEventArgs e) => { LibManager.Add(_DevicesLibrary_ProcessorName.Text); _devices.RefreshDevicesList(); };
            _addDevice.SetRowCol(0, 1);

            ActionButton _CPUEdit = new ActionButton(new Image { Source = new BitmapImage(new Uri($"UI/Assets/CPU_Edit.png", UriKind.Relative)) });
            _CPUEdit.Click += EditCPUName;
            _CPUEdit.SetRowCol(2, 2);

            _DevicesLibrary_ProcessorName = _inputName;
            _DevicesLibrary_ProcessorsListBox = _devices;
            _devices.RefreshDevicesList();
            _container.Children.Add(_inputName);
            _container.Children.Add(_devices);
            _container.Children.Add(_buttonSplitter);
            _container.Children.Add(_CPUEdit);
            _buttonSplitter.Children.Add(_removeDevice);
            _buttonSplitter.Children.Add(_addDevice);
        }

        /// <summary>
        /// Generates settings tab.
        /// </summary>
        /// <param name="_window">Parent window</param>
        /// <param name="_container">Grid container for content</param>
        public static void Settings(ref Grid _container)
        {
            {
                ActionButton _perfTest = new ActionButton("/UI/Assets/PerfTest.png");
                _perfTest.SetRowCol(0, 0);
                _perfTest.Click += (object sender, RoutedEventArgs e) => { new Task(PerfAndTests.Compare).Start(); MessageBox.Show("Calculating time...", "ProcHacker.exe is thinking", MessageBoxButton.OK, MessageBoxImage.Information); };
                _container.Children.Add(_perfTest);
            }

            {
                ActionButton _savePrefs = new ActionButton("UI/Assets/SavePrefs.png");
                _savePrefs.SetRowCol(0, 1);
                _savePrefs.Click += (object sender, RoutedEventArgs e) => { UserPreferences.Settings.currentTheme = (byte)GlobalSettings.currentTheme; UserPreferences.Settings.SavePreferences(); };
                _container.Children.Add(_savePrefs);
            }

            {
                Label _themesLore = new Label
                {
                    Foreground = (SolidColorBrush)MainWindow.GetResource("Title1"),
                    Content = "Theme:"
                };
                _themesLore.SetRowCol(2, 1);
                ListBox _themes = new ListBox()
                {
                    Background = (SolidColorBrush)MainWindow.GetResource("NavPanelOverButton"),
                    Foreground = (SolidColorBrush)MainWindow.GetResource("ActiveText1"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                _themes.SetRowCol(2, 1);
                foreach (string _theme in UITools.Dictionaries.Themes)
                {
                    ListBoxItem _item = new ListBoxItem { Content = System.IO.Path.GetFileName(_theme).Split('.')[0] };
                    _item.MouseDoubleClick += (object _sender, System.Windows.Input.MouseButtonEventArgs e) => UITools.ChangeTheme(_themes.SelectedIndex);
                    _themes.Items.Add(_item);
                }

                _container.Children.Add(_themesLore);
                _container.Children.Add(_themes);
            }
        }
    }
}
