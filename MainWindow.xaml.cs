using ProcHacker.UI.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ProcHacker.Tabs;

namespace ProcHacker
{
	public partial class MainWindow : Window
	{
		// It's mine though
		public const string processorName = "11th Gen Intel(R) Core(TM) i7-11800H @ 2.30GHz";
		// Folder where the images are stored
		const string Assets = "/UI/Assets";
		public List<NavButton> Buttons;
		int activeTab = 0;
		TextBox Txtb1;
		TextBlock Txtb2;
		Image CPUEdit = new Image { Source = new BitmapImage(new Uri($"{Assets}/CPU_Edit.png", UriKind.Relative)) };
		Image CPUView = new Image { Source = new BitmapImage(new Uri($"{Assets}/CPU_Overview.png", UriKind.Relative)) };
		List<Grid> Tabs = null;

		public MainWindow()
		{
			InitializeComponent();
			Tabs = new List<Grid>();
			foreach(Grid _tab in TabContent.Children)
				Tabs.Add(_tab);
			StartUI();
			//RegistryManager.Compare();
		}

		void StartUI()
		{
            InitUI();
            InitButtonsList();
            InitButtons();
            ToggleTab(0);
			((RadioButton)NavContainer.Children[1]).IsChecked = true;
        }

		/// <summary>
		/// Create the Navigation buttons list and the buttons in it.
		/// </summary>
		/// <returns>Nothing lol.</returns>
		private void InitButtonsList()
		{
			Buttons = new List<NavButton>
			{
				new NavButton((SolidColorBrush)GetResource("CPUb"), "CPU",        (Style)GetResource("NavButton"), new Image { Source = new BitmapImage(new Uri("/UI/Assets/CPU_Edit.png", UriKind.Relative)) }),
				new NavButton((SolidColorBrush)GetResource("DevicesLibb"), "Devices",	(Style)GetResource("NavButton"), new Image { Source = new BitmapImage(new Uri("/UI/Assets/CPUBook.png", UriKind.Relative)) }),
				new NavButton((SolidColorBrush)GetResource("Settingsb"), "Settings",   (Style)GetResource("NavButton"), new Image { Source = new BitmapImage(new Uri("/UI/Assets/Settings.png", UriKind.Relative)) })
			};
		}
		/// <summary>
		/// Changes the displayed tab according to the radio button clicked in the left NavPanel.
		/// </summary>
		/// <returns>The current tab.</returns>
		int changeTab(NavButton sender)
		{
			activeTab = sender.Tab;
			ToggleTab(activeTab);
			return activeTab;
		}

		/// <summary>
		/// Hide all tabs except the current one.
		/// </summary>
		void ToggleTab(int _index)
		{
			for (int i = 0; i < Tabs.Count; i++)
				if (i != _index)
					Tabs[i].Visibility = Visibility.Collapsed;
			Tabs[_index].Visibility = Visibility.Visible;
		}

		/// <summary>
		/// Displays all buttons on the screen.
		/// </summary>
		private void InitButtons()
		{
			NavContainer.Children.Clear();
			StackPanel _brand = new StackPanel { Height = 30, Margin = new Thickness(10, 30, 0, 40), Orientation = Orientation.Horizontal };
			_brand.MouseDown += EasterEgg;
			_brand.Children.Add(new Image { Height = 30, HorizontalAlignment = HorizontalAlignment.Left, Source = new BitmapImage(new Uri(@"/UI/Assets/CPU.png", UriKind.Relative)) });
			_brand.Children.Add(new TextBlock { Text = "ProcHacker", FontFamily = new FontFamily("Candara"), Margin = new Thickness(10, 0, 0, 0), Foreground = (Brush)Application.Current.Resources["Title1"], VerticalAlignment = VerticalAlignment.Center, FontSize = 16 });

			NavContainer.Children.Add(_brand);
			for (int i = 0; i < Buttons.Count; i++)
			{
				Buttons[i].Create(NavContainer);
				Buttons[i].Tab = i;
				Buttons[i].onClick += changeTab;
			}
		}

		/// <summary>
		/// An alias to get Resources faster.
		/// </summary>
		/// <returns>The resource specified by the given name.</returns>
		public static object GetResource(string _name) => Application.Current.Resources[_name];

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
				Left = System.Windows.Forms.Cursor.Position.X - Width / 2;
				Top = System.Windows.Forms.Cursor.Position.Y - Height / 2;
			}
			if (e.ChangedButton == MouseButton.Left && WindowState != WindowState.Maximized)
				DragMove();
			if (e.ChangedButton == MouseButton.Middle)
				Close();
		}

		private void Window_Close(object sender, RoutedEventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Displays current CPU info
		/// </summary>
		private void RefreshCPUInfo(object sender, RoutedEventArgs e) => Txtb2.Text = RegistryManager.ReadNoPS(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString"));
		/// <summary>
		/// Write data specified in the textbox to the CPU keys into registry.
		/// </summary>
		private void EditCPUInfo(object sender, RoutedEventArgs e)
		{
			if (RegistryManager.OverWriteNoPS(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString", Txtb1.Text)))
				Txtb1.Foreground = (Brush)GetResource("Settingsb");
			else
				Txtb1.Foreground = (Brush)GetResource("CPUb");
		}

		/// <summary>
		/// Create all Content elements and interactions.
		/// </summary>
		private void InitUI(int _page = 0)
		{
			#region CPU_CONTENT
			{
				CPUContent.Children.Clear();
				ActionButton _EditCPUButton = new ActionButton(CPUEdit);
				_EditCPUButton.Click += EditCPUInfo;
				_EditCPUButton.Content = CPUEdit;
				ActionButton _ViewCPUButton = new ActionButton(CPUView);
				_ViewCPUButton.Click += RefreshCPUInfo;
				_ViewCPUButton.Content = CPUView;
				Grid.SetColumn(_ViewCPUButton, 1);
				Grid.SetRow(_ViewCPUButton, 0);
				TextBox Txtbx1 = new TextBox
				{
					Height = 30,
					Width = 400,
					Foreground = (SolidColorBrush)GetResource("Title1"),
					Background = (SolidColorBrush)GetResource("NavPanelActiveButton"),
					BorderBrush = UITools.UIBrushes.Transparent,
					CaretBrush = (SolidColorBrush)GetResource("Text1"),
					FontFamily = new FontFamily("Cascadia Code SemiBold"),
					FontWeight = FontWeights.Bold,
					TextAlignment = TextAlignment.Center,
					HorizontalContentAlignment = HorizontalAlignment.Center,
					VerticalContentAlignment = VerticalAlignment.Center,
					Name = "Txtb1",
					Margin = new Thickness(10),
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center
				};
				Txtbx1.TextChanged += ResetTextBoxColor;
				Grid.SetColumn(Txtbx1, 0);
				Grid.SetRow(Txtbx1, 1);
				Border _wrapper = new Border { Background = UITools.UIBrushes.Transparent, VerticalAlignment = VerticalAlignment.Center };
				TextBlock Txtbx2 = new TextBlock
				{
					Height = 30,
					Width = 400,
					Foreground = (SolidColorBrush)GetResource("Text1"),
					Background = (SolidColorBrush)GetResource("NavPanelOverButton"),
					FontFamily = new FontFamily("Cascadia Code SemiBold"),
					FontWeight = FontWeights.Bold,
					TextAlignment = TextAlignment.Center,
					Name = "Txtb2",
					Margin = new Thickness(10),
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
				};
				Grid.SetColumn(_wrapper, 1);
				Grid.SetRow(_wrapper, 1);
				_wrapper.Child = Txtbx2;
				CPUContent.Children.Add(_EditCPUButton);
				CPUContent.Children.Add(Txtbx1);
				CPUContent.Children.Add(_ViewCPUButton);
				CPUContent.Children.Add(_wrapper);
				Txtb1 = Txtbx1;
				Txtb2 = Txtbx2;
			}
			#endregion Content relative to CPU actions
			#region DEVICES_LIBRARY_CONTENT
			General.DevicesLibrary(ref DevicesLibrary);
            #endregion
            #region SETTINGS_CONTENT
            General.Settings(ref SettingsContent);
			#endregion
		}

		/// <summary>
		/// Reset Textbox color each time we type a new character inside of it.
		/// </summary>
		private void ResetTextBoxColor(object sender, TextChangedEventArgs e) => Txtb1.Foreground = (SolidColorBrush)GetResource("Title1");
		/// <summary>
		/// Used maybe, one day, for an easter egg. Triggered when we click on the brand name.
		/// </summary>
		private void EasterEgg(object sender, MouseButtonEventArgs e) => new System.Threading.Tasks.Task(new Action(EasterEggMethod)).Start();
		private void EasterEggMethod() => MessageBox.Show("Congrats, you found it !", "ProcHacker easter egg", MessageBoxButton.OK, MessageBoxImage.Exclamation);

    }
}
