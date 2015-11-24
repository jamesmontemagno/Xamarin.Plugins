// Helpers/Settings.cs
using Plugin.Settings.Abstractions;
using System;

namespace Plugin.Settings.Tests.Portable.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class TestSettings
  {
    public static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    public const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    #endregion

    public static Guid GuidSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault("guid_setting", Guid.Empty);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("guid_setting", value);
      }
    }

    public static decimal DecimalSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault("decimal_setting", (decimal)0);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("decimal_setting", value);
      }
    }

    public static int IntSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault("int_setting", (int)0);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("int_setting", value);
      }
    }

    public static float FloatSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault("float_setting", (float)0);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("float_setting", value);
      }
    }

    public static Int64 Int64Setting
    {
      get
      {
        return AppSettings.GetValueOrDefault("int64_setting", (Int64)0);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("int64_setting", value);
      }
    }

    public static Int32 Int32Setting
    {
      get
      {
        return AppSettings.GetValueOrDefault("int32_setting", (Int32)0);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("int32_setting", value);
      }
    }

    public static DateTime? DateTimeSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault<DateTime?>("date_setting");
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("date_setting", value);
      }
    }

    public static double DoubleSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault("double_setting", (double)0);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue("double_setting", value);
      }
    }

    public static bool BoolSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault("bool_setting", false);
      }
      set
      {
        AppSettings.AddOrUpdateValue("bool_setting", value);
      }
    }

    public static string StringSetting
    {
      get
      {
        return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
      }
      set
      {
        //if value has changed then save it!
        AppSettings.AddOrUpdateValue(SettingsKey, value);
      }
    }

    public static void Remove(string key)
    {
      AppSettings.Remove(key);
    }

  }
}