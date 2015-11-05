// Helpers/Settings.cs
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace TestAppForms.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        const string FirstRunKey = "first_run";
        static bool FirstRunDefault = true;

        #endregion


        public static string InsightsKey
        {
            get { return "0d146df0686f2586838e7071e5916b5cd6e63a95"; }
        }

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
            }
        }

        public static bool FirstRun
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(FirstRunKey, FirstRunDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(FirstRunKey, value);
            }
        }

    }
}