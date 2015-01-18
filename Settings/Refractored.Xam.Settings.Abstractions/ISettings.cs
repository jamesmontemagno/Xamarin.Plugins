
using System;

namespace Refractored.Xam.Settings.Abstractions
{
  /// <summary>
  /// Main interface for settings
  /// </summary>
  public interface ISettings
  {
    /// <summary>
    /// Gets the current value or the default that you specify.
    /// </summary>
    /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
    /// <param name="key">Key for settings</param>
    /// <param name="defaultValue">default value if not set</param>
    /// <returns>Value or default</returns>
    T GetValueOrDefault<T>(string key, T defaultValue = default(T));


    /// <summary>
    /// Adds or updates the value 
    /// </summary>
    /// <param name="key">Key for settting</param>
    /// <param name="value">Value to set</param>
    /// <returns>True of was added or updated and you need to save it.</returns>
    bool AddOrUpdateValue<T>(string key, T value);

    /// <summary>
    /// Adds or updates the value 
    /// </summary>
    /// <param name="key">Key for settting</param>
    /// <param name="value">Value to set</param>
    /// <returns>True of was added or updated and you need to save it.</returns>
    [Obsolete("This method is now obsolete, please use generic version as this may be removed in the future.")]
    bool AddOrUpdateValue(string key, Object value);

    /// <summary>
    /// Removes a desired key from the settings
    /// </summary>
    /// <param name="key">Key for setting</param>
    void Remove(string key);

    /// <summary>
    /// Saves any changes out.
    /// </summary>
    [Obsolete("Save is deprecated and settings are automatically saved when AddOrUpdateValue is called.")]
    void Save();
  }
}
