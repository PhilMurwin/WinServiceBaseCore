using System.Configuration;

namespace WinServiceBaseCore.App_Entry
{
    public static class ConfigKeys
    {
        #region Basic Time Logger Settings
        public static bool BasicTimeLogger
        {
            get
            {
                bool.TryParse( GetConfigKey( "BasicTimeLogger" ), out bool boolParse );
                return boolParse;
            }
        }
        public static int BasicTimeLoggerFrequency
        {
            get
            {
                int.TryParse( GetConfigKey( "BasicTimeLoggerFrequency" ), out int intParse );
                return intParse;
            }
        }
        #endregion Basic Time Logger Settings

        #region Email Test Settings
        public static bool EmailTest
        {
            get
            {
                bool.TryParse( GetConfigKey( "EmailTest" ), out bool boolParse );
                return boolParse;
            }
        }
        #endregion Email Test Settings

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
