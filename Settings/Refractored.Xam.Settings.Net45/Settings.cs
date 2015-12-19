using Plugin.Settings.Abstractions;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace Plugin.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingsImplementation : ISettings
    {
        private static IsolatedStorageFile Store
        {
            get { return IsolatedStorageFile.GetMachineStoreForAssembly(); }
        }

        private readonly object locker = new object();

        /// <summary>
        /// Add or Upate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue<T>(string key, T value)
        {
            if (value == null)
            {
                var exists = Store.FileExists(key);

                Remove(key);

                return exists;
            }

            var type = value.GetType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments.FirstOrDefault();
            }


            if ((type == typeof(string)) ||
                (type == typeof(decimal)) ||
                (type == typeof(double)) ||
                (type == typeof(Single)) ||
                (type == typeof(DateTime)) ||
                (type == typeof(Guid)) ||
                (type == typeof(bool)) ||
                (type == typeof(Int32)) ||
                (type == typeof(Int64)) ||
                (type == typeof(byte)))
            {
                lock (locker)
                {
                    string str;

                    if (value is decimal)
                    {
                        return AddOrUpdateValue(key, Convert.ToString(Convert.ToDecimal(value), System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (value is DateTime)
                    {
                        return AddOrUpdateValue(key, Convert.ToString(-(Convert.ToDateTime(value)).ToUniversalTime().Ticks, System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                        str = Convert.ToString(value);

                    string oldValue = null;

                    if (Store.FileExists(key))
                    {
                        using (var stream = Store.OpenFile(key, FileMode.Open))
                        {
                            using (var sr = new StreamReader(stream))
                            {
                                oldValue = sr.ReadToEnd();
                            }
                        }
                    }

                    using (var stream = Store.OpenFile(key, FileMode.Create, FileAccess.Write))
                    {
                        using (var sw = new StreamWriter(stream))
                        {
                            sw.Write(str);
                        }
                    }

                    return oldValue != str;
                }
            }

            throw new ArgumentException(string.Format("Value of type {0} is not supported.", type.Name));
        }
    
        /// <summary>
        /// Get Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            object value = null;
            lock (locker)
            {
                string str = null;

                // If the key exists, retrieve the value.
                if (Store.FileExists(key))
                {
                    using (var stream = Store.OpenFile(key, FileMode.Open))
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            str = sr.ReadToEnd();
                        }
                    }
                }

                if (str == null)
                    return defaultValue;

                var type = typeof(T);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GenericTypeArguments.FirstOrDefault();
                }

                if (type == typeof(string))
                    value = str;

                else if (type == typeof(decimal))
                {
                    
                    string savedDecimal = Convert.ToString(str);
                    

                    value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);

                    return null != value ? (T)value : defaultValue;
                    
                }

                else if (type == typeof(double))
                {
                    value = Convert.ToDouble(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else if (type == typeof(Single))
                {
                    value = Convert.ToSingle(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else if (type == typeof(DateTime))
                {
                    
                    var ticks = Convert.ToInt64(str, System.Globalization.CultureInfo.InvariantCulture);
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
                    

                    return (T)value;
                }

                else if (type == typeof(Guid))
                {
                    Guid guid;
                    if (Guid.TryParse(str, out guid))
                        value = guid;
                }

                else if (type == typeof(bool))
                {
                    value = Convert.ToBoolean(str);
                }

                else if (type == typeof(Int32))
                {
                    value = Convert.ToInt32(str);
                }

                else if (type == typeof(Int64))
                {
                    value = Convert.ToInt64(str);
                }

                else if (type == typeof(byte))
                {
                    value = Convert.ToByte(str);
                }

                else
                {
                    throw new ArgumentException(string.Format("Value of type {0} is not supported.", typeof(T).Name));
                }
            }

            return null != value ? (T)value : defaultValue;
        }

        /// <summary>
        /// Remove key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (Store.FileExists(key))
                Store.DeleteFile(key);
        }
    }
}
