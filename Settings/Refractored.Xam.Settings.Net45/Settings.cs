using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refractored.Xam.Settings.Abstractions;

namespace Refractored.Xam.Settings
{
    /// <summary>
    /// Main ISettings Implementation
    /// </summary>
    public class Settings : ISettings
    {
        private static IsolatedStorageFile Store
        {
            get { return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null); }
        }

        private readonly object locker = new object();

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

                var type = typeof (T);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
                {
                    type = type.GenericTypeArguments.FirstOrDefault();
                }

                if (type == typeof (string))
                    value = str;

                else if (type == typeof(decimal))
                {
                    value = Convert.ToDecimal(str, System.Globalization.CultureInfo.InvariantCulture);
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
                    Int64 ticks = Convert.ToInt64(str, System.Globalization.CultureInfo.InvariantCulture);
                    if (ticks == -1)
                        value = defaultValue;
                    else
                        value = new DateTime(ticks);
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

                else if (type == typeof (byte))
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

        public bool AddOrUpdateValue<T>(string key, T value)
        {
            return InternalAddOrUpdateValue(key, value);
        }

        [Obsolete("This method is now obsolete, please use generic version as this may be removed in the future.")]
        public bool AddOrUpdateValue(string key, object value)
        {
            return InternalAddOrUpdateValue(key, value);
        }

        public void Remove(string key)
        {
            if (Store.FileExists(key))
                Store.DeleteFile(key);
        }

        [Obsolete("Save is deprecated and settings are automatically saved when AddOrUpdateValue is called.")]
        public void Save()
        {
            
        }

        private bool InternalAddOrUpdateValue(string key, object value)
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


            if ((type == typeof (string)) ||
                (type == typeof (decimal)) ||
                (type == typeof (double)) ||
                (type == typeof (Single)) ||
                (type == typeof (DateTime)) ||
                (type == typeof (Guid)) ||
                (type == typeof (bool)) ||
                (type == typeof (Int32)) ||
                (type == typeof (Int64)) ||
                (type == typeof (byte)))
            {
                lock (locker)
                {
                    string str;

                    if (type == typeof (DateTime))
                        str = Convert.ToString(((DateTime)value).Ticks);
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

                    using (var stream = Store.OpenFile(key, FileMode.OpenOrCreate, FileAccess.Write))
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
    }
}
