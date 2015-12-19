

using Windows.Storage;
using Plugin.Settings.Abstractions;
using System;

namespace Plugin.Settings
{
    /// <summary>
    /// Main ISettings Implementation
    /// </summary>
    public class SettingsImplementation : ISettings
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
                if (typeof(T) == typeof(decimal))
                {
                    string savedDecimal;
                    // If the key exists, retrieve the value.
                    if (AppSettings.Values.ContainsKey(key))
                    {
                        savedDecimal = Convert.ToString(AppSettings.Values[key]);
                    }
                    // Otherwise, use the default value.
                    else
                    {
                        savedDecimal = defaultValue == null ? default(decimal).ToString() : defaultValue.ToString();
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
                        savedTime = Convert.ToString(AppSettings.Values[key]);
                    }

                    if (string.IsNullOrWhiteSpace(savedTime))
                    {
                        value = defaultValue;
                    }
                    else
                    {
                        var ticks = Convert.ToInt64(savedTime, System.Globalization.CultureInfo.InvariantCulture);
                        if (ticks >= 0)
                        {
                            //Old value, stored before update to UTC values
                            value = new DateTime(ticks);
                        }
                        else
                        {
                            //New value, UTC
                            value = new DateTime(-ticks, DateTimeKind.Utc);
                        }
                    }

                    return (T)value;
                }

                // If the key exists, retrieve the value.
                if (AppSettings.Values.ContainsKey(key))
                {
                    var tempValue = AppSettings.Values[key];
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
                    return AddOrUpdateValue(key, Convert.ToString(-(Convert.ToDateTime(value)).ToUniversalTime().Ticks, System.Globalization.CultureInfo.InvariantCulture));
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
