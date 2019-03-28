using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Borman.ZuSharp
{
    using Microsoft.Win32;

    /// <summary>
    /// Utility.
    /// </summary>
    public class StartWithWindowsUtil
    {
        private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public string KeyName { get; set; }
        public string AssemblyLocation { get; set; }

        public StartWithWindowsUtil(string keyName, string assemblyLocation)
        {
            KeyName = keyName;
            AssemblyLocation = assemblyLocation;
        }

        /// <summary>
        /// Sets the autostart value for the assembly.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public void SetAutoStart()
        {
            if (IsAutoStartEnabled())
            {
                return;
            }

            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.SetValue(KeyName, AssemblyLocation);
        }

        /// <summary>
        /// Returns whether auto start is enabled.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        /// <param name="assemblyLocation">Assembly location (e.g. Assembly.GetExecutingAssembly().Location)</param>
        public bool IsAutoStartEnabled()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
            if (key == null)
                return false;

            string value = (string)key.GetValue(KeyName);
            if (value == null)
                return false;

            return (value == AssemblyLocation);
        }

        /// <summary>
        /// Unsets the autostart value for the assembly.
        /// </summary>
        /// <param name="keyName">Registry Key Name</param>
        public void UnSetAutoStart()
        {
            if (!IsAutoStartEnabled())
            {
                return;
            }

            RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
            key.DeleteValue(KeyName);
        }
    }
}
