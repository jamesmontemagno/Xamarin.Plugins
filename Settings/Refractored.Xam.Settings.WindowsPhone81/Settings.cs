

using Windows.Storage;
using Refractored.Xam.Settings.Abstractions;
using System;

namespace Refractored.Xam.Settings
{
  /// <summary>
  /// Main ISettings Implementation
  /// </summary>
  public class Settings : ISettings
  {
    private static ApplicationDataContainer AppSettings
    {
      get { return ApplicationData.Current.LocalSettings; }
    }

    private readonly object locker = new object();

    /// <summary>
    /// Gets the current value or the default that you specify.
    /// </summary>
    /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
    /// <param name="key">Key for settings</param>
    /// <param name="defaultValue">default value if not set</param>
    /// <returns>Value or default</returns>
    public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
    {
      object value;
      lock (locker)
      {
        if(typeof(T) == typeof(decimal))
        {
          string savedDecimal;
          // If the key exists, retrieve the value.
          if (AppSettings.Values.ContainsKey(key))
          {
            savedDecimal = (string)AppSettings.Values[key];
          }
          // Otherwise, use the default value.
          else
          {
            savedDecimal = defaultValue.ToString();
          }

          value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);

          return null != value ? (T)value : defaultValue;
        }
        else if (typeof(T) == typeof(DateTime))
        {
          string savedTime = null;
          // If the key exists, retrieve the value.
          if (AppSettings.Values.ContainsKey(key))
          {
            savedTime = (string)AppSettings.Values[key];
          }

          var ticks = string.IsNullOrWhiteSpace(savedTime) ? -1 : Convert.ToInt64(savedTime, System.Globalization.CultureInfo.InvariantCulture);
          if (ticks == -1)
            value = defaultValue;
          else
            value = new DateTime(ticks);

          return null != value ? (T)value : defaultValue;
        }
       
        // If the key exists, retrieve the value.
        if (AppSettings.Values.ContainsKey(key))
        {
          value = (T)AppSettings.Values[key];
        }
        // Otherwise, use the default value.
        else
        {
          value = defaultValue;
        }
      }

      return null != value ? (T)value : defaultValue;
    }

    /// <summary>
    /// Adds or updates a value
    /// </summary>
    /// <param name="key">key to update</param>
    /// <param name="value">value to set</param>
    /// <returns>True if added or update and you need to save</returns>
    public bool AddOrUpdateValue<T>(string key, T value)
    {
      return InternalAddOrUpdateValue(key, value);
    }



    /// <summary>
    /// Adds or updates the value 
    /// </summary>
    /// <param name="key">Key for settting</param>
    /// <param name="value">Value to set</param>
    /// <returns>True of was added or updated and you need to save it.</returns>
    [Obsolete("This method is now obsolete, please use generic version as this may be removed in the future.")]
    public bool AddOrUpdateValue(string key, object value)
    {
      return InternalAddOrUpdateValue(key, value);
    }

    private bool InternalAddOrUpdateValue(string key, object value)
    {
      bool valueChanged = false;
      lock (locker)
      {

        if (value is decimal)
        {
          return AddOrUpdateValue(key, Convert.ToString(Convert.ToDecimal(value), System.Globalization.CultureInfo.InvariantCulture));
        }
        else if (value is DateTime)
        {
          return AddOrUpdateValue(key, Convert.ToString((Convert.ToDateTime(value)).Ticks, System.Globalization.CultureInfo.InvariantCulture));
        }


        // If the key exists
        if (AppSettings.Values.ContainsKey(key))
        {

          // If the value has changed
          if (AppSettings.Values[key] != value)
          {
            // Store key new value
            AppSettings.Values[key] = value;
            valueChanged = true;
          }
        }
        // Otherwise create the key.
        else
        {
          AppSettings.CreateContainer(key, ApplicationDataCreateDisposition.Always);
          AppSettings.Values[key] = value;
          valueChanged = true;
        }
      }

      return valueChanged;
    }

    /// <summary>
    /// Saves any changes out.
    /// </summary>
    [Obsolete("Save is deprecated and settings are automatically saved when AddOrUpdateValue is called.")]
    public void Save()
    {
      //nothing to do it is automatic
    }

    /// <summary>
    /// Removes a desired key from the settings
    /// </summary>
    /// <param name="key">Key for setting</param>
    public void Remove(string key)
    {
      lock (locker)
      {
        // If the key exists remove
        if (AppSettings.Values.ContainsKey(key))
        {
          AppSettings.Values.Remove(key);
        }
      }
    }
  }
}
