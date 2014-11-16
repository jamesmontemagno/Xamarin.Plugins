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
          value = (T)IsoSettings[key];
        }
        // Otherwise, use the default value.
        else
        {
          value = defaultValue;
        }
      }

      return value;
    }

    /// <summary>
    /// Adds or updates the value 
    /// </summary>
    /// <param name="key">Key for settting</param>
    /// <param name="value">Value to set</param>
    /// <returns>True of was added or updated and you need to save it.</returns>
    public bool AddOrUpdateValue(string key, object value)
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

      if(valueChanged)
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

  }
}
