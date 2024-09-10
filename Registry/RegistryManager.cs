using System;
using System.Diagnostics;
using System.Windows;

namespace ProcHacker
{
    internal static class RegistryManager
    {
        static string _tmpFilePath = System.IO.Path.GetFullPath("procinfo.txt");
        /// <summary>
        /// Overrides or just write a new Registry key using Powershell.
        /// </summary>
        /// <param name="_infos"></param>
        /// <returns>True if the key has successfully been wrote into the registry, False if not.</returns>
        public static bool OverWriteKey(Key _infos)
        {
            string _command = $"Set-ItemProperty {_infos.ToCommandString()}";
            Process _powerShell = PowerShellQuickStart(_command);
            _powerShell.Start();
            string _errors = _powerShell.StandardError.ReadToEnd();
            _powerShell.WaitForExit();
            if (!string.IsNullOrEmpty(_errors) || ReadKey(_infos) != _infos.Value.Trim())
            {
                OutputRegistryError(_errors + $" .{_infos.Value}. != \n.{ReadKey(_infos)}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Read the specified Key in the Registry.
        /// </summary>
        /// <param name="_readable">Key to look for in the registry</param>
        /// <returns>The specified Key content stored in the Registry.</returns>
        public static string ReadKey(Key _readable)
        {
            string _command = $"Get-ItemProperty {_readable.ToGetString()}";
            Process _powerShell = PowerShellQuickStart(_command);
            _powerShell.Start();
            string _output = _powerShell.StandardOutput.ReadToEnd();
            string _errors = _powerShell.StandardError.ReadToEnd();
            _powerShell.WaitForExit();
            if (!(string.IsNullOrEmpty(_errors) || string.IsNullOrEmpty(_output)))
                OutputRegistryError($"Couldn't get any output or following error(s) was found:\n{_errors}");
            return _output.Split(':')[1].Split('\n')[0].Trim().Replace("PSPath", "");
        }

        public static bool OverWriteNoPS(Key _infos)
        {
            Process _regedit = RegEdit(_infos, false);
            _regedit.Start();
            string _err = _regedit.StandardError.ReadToEnd();
            _regedit.WaitForExit();
            if (!string.IsNullOrEmpty(_err) || ReadNoPS(_infos) != _infos.Value.Trim())
            {
                OutputRegistryError(_err);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Optimized function to look for a registry key.
        /// </summary>
        /// <param name="_infos">Registry key to look for</param>
        /// <returns>The key content.</returns>
        public static string ReadNoPS(Key _infos)
        {
            Process _regedit = RegEdit(_infos, true, true);
            _regedit.Start();
            string _output = _regedit.StandardOutput.ReadToEnd();
            string _err = _regedit.StandardError.ReadToEnd();
            _regedit.StandardOutput.Close();
            _regedit.WaitForExit();
            _regedit.Dispose();
            if (!string.IsNullOrEmpty(_err))
                OutputRegistryError(_err);
            return _output.Substring(_output.IndexOf("REG_SZ") + 6).Trim();
        }


        /// <summary>
        /// Creates an instance of Powershell with the specified string Command.
        /// </summary>
        /// <param name="_command"></param>
        /// <returns>That created instance of Powershell.</returns>
        static Process PowerShellQuickStart(string _command) =>
            new Process()
            {
                StartInfo =
                    new ProcessStartInfo()
                    {
                        FileName = "powershell.exe",
                        Arguments = $"Powershell -NoProfile -ExecutionPolicy Bypass -Command \'{_command}\'",
                        Verb = "runas",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        StandardOutputEncoding = System.Text.Encoding.UTF8,
                        StandardErrorEncoding = System.Text.Encoding.UTF8,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
            };

        /// <summary>
        /// Returns a process running Reg.exe to alter the registry depending on if it's for reading or writing.
        /// </summary>
        /// <param name="_infos">Key's infos</param>
        /// <param name="_reading">True if you want to read the key, false if you want to read it</param>
        /// <param name="_displayOutput">True if you want to retrieve output, false if you want to write a data file.</param>
        /// <returns></returns>
        static Process RegEdit(Key _infos, bool _reading, bool _displayOutput = false) =>
            new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    Arguments =
                        _reading ?
                            $"/c reg query \"{_infos.GetPath().Replace(":","")}\" /v \"{Key.FormatForCmd(_infos.Name)}\" { (_displayOutput ? "\0" : $"| findstr /R /C:\"{Key.FormatForCmd(_infos.Name)}\" > \"{Key.FormatForCmd(_tmpFilePath) }\"")}" :
                            $"/c reg add \"{_infos.GetPath().Replace(":","")}\" /v {Key.FormatForCmd(_infos.Name)} /t REG_SZ /d \"{Key.FormatForCmd(_infos.Value)}\" /f",
                    Verb = "runas",
                    WorkingDirectory = @"C:\Windows\system32",
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    RedirectStandardError = true,
                    StandardErrorEncoding = System.Text.Encoding.UTF8,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

        static void DisplayCopy(string _idc)
        {
            Clipboard.SetText(_idc);
            MessageBox.Show(_idc);
        }

        /// <summary>
        /// Alias for a MessageBox that shows an error.
        /// </summary>
        /// <param name="_error">Error to display.</param>
        private static void OutputRegistryError(string _error) => MessageBox.Show(_error, "Registry Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
