using ProcHacker.Tools;
using System.Security.Permissions;

namespace ProcHacker.UserPreferences
{
    /// <summary>
    /// Each byte is for a setting. <br/>
    /// First is for the theme.
    /// </summary>
    static class Settings
    {
        /// <summary>
        /// 0: DeepSea <br/>
        /// 1: Reddish <br/>
        /// 2: Global warning <br/>
        /// 3: Dark <br/>
        /// </summary>
        public static byte currentTheme = 0;
        public static string settingsPath { get; private set; } = "user.pref";

        /// <summary>
        /// Save user preferences in a file.
        /// </summary>
        /// <returns>an error code, <c>0</c> if no error occured, <c>1</c> if the file has been created during the process.</returns>
        /// <exception cref="2 Error while writing">File content isn't as expected</exception>
        public static byte SavePreferences()
        {
            byte _result = (byte)(0b1 & (System.IO.File.Exists(settingsPath) ? 1 : 0));
            byte[] _data = 
            { 
                currentTheme 
            }; 
            System.IO.File.WriteAllBytes(settingsPath, _data);
            if (System.IO.File.ReadAllBytes(settingsPath)[0] == _result)
                return _result;
            if(ByteTools.Compare(_data, System.IO.File.ReadAllBytes(settingsPath)))
                System.Windows.MessageBox.Show("Preferences were successfully saved !", "ProcHacker", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            return _result.And(0b10);
        }

        /// <summary>
        /// Updates all current settings using the saved ones.
        /// </summary>
        public static void Update()
        {
            if (!System.IO.File.Exists(settingsPath)) 
                return;
            byte _settings = System.IO.File.ReadAllBytes(settingsPath)[0];
            currentTheme = _settings.And(0b11);
            GlobalSettings.currentTheme = currentTheme;
        }
    }
}
