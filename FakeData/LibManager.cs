using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProcHacker.FakeData
{
	internal class LibManager
	{
		public const char Separator = 'µ';
		public static List<Processor> Processors { get; private set; } = new List<Processor>();

		public static void UpdateList()
		{
			Processors.Clear();
			if (!System.IO.File.Exists(GlobalSettings.DevicesLibPath))
				System.IO.File.WriteAllText(GlobalSettings.DevicesLibPath, MainWindow.processorName);
			string _rawLib = System.IO.File.ReadAllText(GlobalSettings.DevicesLibPath);
			string[] _devices = _rawLib.Split(Separator);
			for (int i = 1; i < _devices.Length; i++)
				Processors.Add(new Processor(_devices[i]));
		}

		public static void Add(Processor _processor)
		{
			UpdateList();
			if(!string.IsNullOrEmpty(_processor.Name))
				Processors.Add(_processor);
			System.IO.File.WriteAllText(GlobalSettings.DevicesLibPath, FormatProcList());
		}

		public static bool Remove(Processor _processor)
		{
			int _initialCount = Processors.Count;
			if (Processors.Contains(_processor))
			{
				Processors.Remove(_processor);
				return true;
			}
			for (int i = 0; i < Processors.Count; i++)
				if (Processors[i].Name == _processor.Name)
					Processors.RemoveAt(i);
			System.IO.File.WriteAllText(GlobalSettings.DevicesLibPath, FormatProcList());
			return _initialCount == (Processors.Count +1);
		}

		public static bool Remove(int _index)
		{
			if (_index >= 0 && Processors.Count >= _index)
			{
				int _initialCount = Processors.Count;
				Processors.RemoveAt(_index);
				if(_initialCount == (Processors.Count +1))
				{
					System.IO.File.WriteAllText(GlobalSettings.DevicesLibPath, FormatProcList());
					return true;
				}
			}
			return false;
		}

		public static string FormatProcList()
		{
			string _result = "";
			foreach (Processor processor in Processors)
				_result += $"{Separator}{processor.Name}";
			return _result;
		}
	}
}
