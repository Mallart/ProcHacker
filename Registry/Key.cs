namespace ProcHacker
{
    class Key
    {
        /// <summary>
        /// Enum storing all keys relative to KeyPath Dictionnary.
        /// </summary>
        public enum KeyType
        {
            ProcessorName
        }
        /// <summary>
        /// With the key's type name, finds the real key location in registry. Usage: <code>KeyPath["KeyType.ProcessorName"]</code>
        /// This feature exists in case more Keys could be modified in the future.
        /// </summary>
        public static System.Collections.Generic.Dictionary<KeyType, string> KeyPath { get; private set; } = new System.Collections.Generic.Dictionary<KeyType, string>()
        {
            { KeyType.ProcessorName, "HKLM:/HARDWARE/DESCRIPTION/System/CentralProcessor/0" }
        };

        /// <summary>
        /// The key's name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The key's value.
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// The Registry path to get to the key.
        /// </summary>
        public string Path { get ; private set; }

        /// <summary>
        /// Returns a new Key.
        /// </summary>
        /// <param name="_path">The registry path where the key is stored</param>
        /// <param name="_name">The key name</param>
        /// <param name="_value">The Key's value. If null or empty, means it's used to look for a key value in the registry.</param>
        public Key(string _path, string _name, string _value="")
        {
            Name    = _name;
            Value   = _value;
            Path    = _path;
        }

        /// <summary>
        /// Escapes all characters to avoid conflict with Microsoft PowerShell.
        /// </summary>
        /// <param name="_command">Command to format to Windows PowerShelll format</param>
        /// <returns>A formatted command for Microsoft Powershell.</returns>
        string FormatForPowerShell(string _command) => _command.Replace(" ", "` ").Replace("(", "`(").Replace(")", "`)").Replace("@", "`@").Replace("[", "`[").Replace("]", "`]").Replace("{", "`{").Replace("}", "`}");
        /// /// <summary>
        /// Escapes all characters to avoid conflict with Windows Command Prompt.
        /// </summary>
        /// <param name="_command">Command to escape all necessary characters for Windows Command Prompt</param>
        /// <returns>A formatted command for Microsoft Powershell.</returns>
        public static string FormatForCmd(string _command) => _command.Replace("\"", "\\\"").Replace("/", "\\");
        /// <summary>
        /// Transforms the Key into a string Powershell command to set the Registry key.
        /// </summary>
        /// <returns>The key part of the command to set the registry key.</returns>
        public string ToCommandString() => $"-Path \"{Path.Replace('/','\\')}\" -Name \"{Name}\" -Value \"{FormatForPowerShell(Value)}\"";
        /// <summary>
        /// Transforms the Key into a string to get the Registry key.
        /// </summary>
        /// <returns>The key part of the command to get the registry key.</returns>
        public string ToGetString() => $"-Path \"{Path.Replace('/', '\\')}\" -Name \"{Name}\"";
        public string GetPath() => Path.Replace('/', '\\');
    }
}
