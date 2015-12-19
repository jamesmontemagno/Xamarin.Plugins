Settings Plugin Readme

Changelog:
[2.1.0]
-Remove help file and add in readme.txt
-.NET 4.5 Implementation for unit testing
[2.0.0]
-Breaking changes:
--New namespace - Plugin.Settings
--Remove Obsolete Methods
-Add UWP Support
-Enhanced DateTime (now saved to UTC)

Learn More:
http://www.github.com/jamesmontemagno/Xamarn.Plugins
http://www.xamarin.com/plugins

Created by James Montemagno:
http://twitter.com/jamesmontemagno
http://www.motzcod.es


Ensure that you install NuGet into PCL and see Helpers/Settings.cs

If you are installing this in a normal project and not using a pcl create a new file called Settings.cs or whatever you want and copy this code in:


// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace $rootnamespace$.Helpers
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

    #endregion


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

  }
}
