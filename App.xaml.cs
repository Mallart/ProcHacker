using ProcHacker.UI.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProcHacker
{
	public partial class App : Application
	{
		private void OnStartup(object _sender, StartupEventArgs e)
		{
			UserPreferences.Settings.Update();
			UITools.ChangeTheme(UserPreferences.Settings.currentTheme, false);
		}
	}
}
