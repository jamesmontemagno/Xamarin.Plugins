using System.IO.IsolatedStorage;
using Refractored.Xam.Settings.Abstractions;
using System;

namespace Refractored.Xam.Settings
{
  /// <summary>
  /// Main settings implementation
  /// </summary>
  public class Settings : ISettings
  {
    static IsolatedStorageSettings IsoSettings { get { return IsolatedStorageSettings.ApplicationSettings; } }
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
      T value;
      lock (locker)
      {
        // If the key exists, retrieve the value.
        if (IsoSettings.Contains(key))
        {
          var tempValue = IsoSettings[key];
          if (tempValue != null)
            value = (T)tempValue;
          else
            value = defaultValue;
        }
        // Otherwise, use the default value.
        else
        {
          value = defaultValue;
        }
      }

      return null != value ? value : defaultValue;
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
        // If the key exists
        if (IsoSettings.Contains(key))
        {

          // If the value has changed
          if (IsoSettings[key] != value)
          {
            // Store key new value
            IsoSettings[key] = value;
            valueChanged = true;
          }
        }
        // Otherwise create the key.
        else
        {
          IsoSettings.Add(key, value);
          valueChanged = true;
        }
      }

      if (valueChanged)
      {
        lock (locker)
        {
          IsoSettings.Save();
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
        if (IsoSettings.Contains(key))
        {
          IsoSettings.Remove(key);
        }
      }
    }

  }
}
