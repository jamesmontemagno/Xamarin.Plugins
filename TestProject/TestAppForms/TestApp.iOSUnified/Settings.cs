using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace TestApp.iOSUnified.Helpers
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
    private const string SettingsKey2 = "settings_key2";
    private static readonly bool SettingsDefault2 = false;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue(SettingsKey, value);
      }
    }


    public static bool GeneralSettings2
    {
      get
      {
        return AppSettings.GetValueOrDefault(SettingsKey2, SettingsDefault2);
      }
      set
      {
        AppSettings.AddOrUpdateValue(SettingsKey2, value);
      }
    }

  }
}