using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProcHacker.Registry;

namespace ProcHacker.Registry
{
    internal class PerfAndTests
    {
        /// <summary>
        /// Compare the time took to execute the same tasks with two methods.
        /// </summary>
        public static void Compare()
        {
            string _procName = RegistryManager.ReadNoPS(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString"));
            // Writing
            long ms = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            RegistryManager.OverWriteKey(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString", "intaile caure 1"));
            long Write_PSTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - ms;
            ms = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            RegistryManager.OverWriteNoPS(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString", "intaile caure 2"));
            long Write_regTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - ms;

            // Reading
            ms = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            RegistryManager.ReadKey(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString"));
            long Read_PSTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - ms;
            ms = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            RegistryManager.ReadNoPS(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString"));
            long Read_regTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - ms;
            RegistryManager.OverWriteNoPS(new Key(Key.KeyPath[Key.KeyType.ProcessorName], "ProcessorNameString", _procName));
            string _result = $"Reg.exe is {(Write_PSTime / Write_regTime + Read_PSTime / Read_regTime) / 0.02}% faster than Powershell method.";
            System.Windows.MessageBox.Show($"Old method (Powershell) took {Read_PSTime} milliseconds to execute and new (Reg.exe) took {Read_regTime} milliseconds to read the exact same thing. \n" +
                $"Old method (Powershell) took {Write_PSTime} milliseconds to execute and new (Reg.exe) took {Write_regTime} milliseconds to write the exact same thing. \n{_result}", "Performance Test", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
