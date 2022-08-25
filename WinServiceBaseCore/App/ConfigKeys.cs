using System.Configuration;

namespace WinServiceBaseCore.App
{
    public static class ConfigKeys
    {
        #region Basic Time Logger Settings
        public static bool BasicTimeLogger => bool.Parse(GetConfigKey("BasicTimeLogger"));

        public static int BasicTimeLoggerFrequency => int.Parse(GetConfigKey("BasicTimeLoggerFrequency"));
        #endregion Basic Time Logger Settings

        #region Email Test Settings
        public static bool EmailTest => bool.Parse(GetConfigKey("EmailTest"));

        public static int EmailTestFrequency => int.Parse(GetConfigKey("EmailTestFrequency"));
        #endregion Email Test Settings

        #region WriteToFile
        public static bool WriteToFile => bool.Parse(GetConfigKey("WriteToFile"));
        public static int WriteToFileFrequency => int.Parse(GetConfigKey("WriteToFileFrequency"));
        public static string WriteToFileDir => GetConfigKey("WriteToFileDir");
        #endregion WriteToFile

        /// <summary>
        /// Helper method for getting config keys
        /// <para>Refreshes the appSettings section of the config before retrieving a setting</para>
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static string GetConfigKey( string setting )
        {
            ConfigurationManager.RefreshSection( "appSettings" );
            return ConfigurationManager.AppSettings[setting];
        }
    }
}
