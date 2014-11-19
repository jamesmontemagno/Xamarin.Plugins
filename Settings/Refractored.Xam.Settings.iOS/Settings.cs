/*
 * MvxSettings:
 * Copyright (C) 2014 Refractored: 
 * 
 * Contributors:
 * http://github.com/JamesMontemagno
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */

using System;
#if __UNIFIED__
using Foundation;
#else
using MonoTouch.Foundation;
#endif
using Refractored.Xam.Settings.Abstractions;

namespace Refractored.Xam.Settings
{
  /// <summary>
  /// Main implementation for ISettings
  /// </summary>
  public class Settings : ISettings
  {

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
      lock (locker)
      {
        if (NSUserDefaults.StandardUserDefaults[key] == null)
          return defaultValue;

        Type typeOf = typeof(T);
        if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          typeOf = Nullable.GetUnderlyingType(typeOf);
        }
        object value = null;
        var typeCode = Type.GetTypeCode(typeOf);
        var defaults = NSUserDefaults.StandardUserDefaults;
        switch (typeCode)
        {
          case TypeCode.Decimal:
            var savedDecimal = defaults.StringForKey(key);
            value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);
            break;
          case TypeCode.Boolean:
            value = defaults.BoolForKey(key);
            break;
          case TypeCode.Int64:
            var savedInt64 = defaults.StringForKey(key);
            value = Convert.ToInt64(savedInt64, System.Globalization.CultureInfo.InvariantCulture);
            break;
          case TypeCode.Double:
            value = defaults.DoubleForKey(key);
            break;
          case TypeCode.String:
            value = defaults.StringForKey(key);
            break;
          case TypeCode.Int32:
#if __UNIFIED__
            value = (Int32)defaults.IntForKey(key);
#else
            value = defaults.IntForKey(key);
#endif
            break;
          case TypeCode.Single:
#if __UNIFIED__
            value = (float)defaults.FloatForKey(key);
#else
             value = defaults.FloatForKey(key);
#endif
           
            break;

          case TypeCode.DateTime:
            var savedTime = defaults.StringForKey(key);
            var ticks = string.IsNullOrWhiteSpace(savedTime) ? -1 : Convert.ToInt64(savedTime, System.Globalization.CultureInfo.InvariantCulture);
            if (ticks == -1)
              value = defaultValue;
            else
              value = new DateTime(ticks);
            break;
          default:

            if (defaultValue is Guid)
            {
              var outGuid = Guid.Empty;
              var savedGuid = defaults.StringForKey(key);
              if(string.IsNullOrWhiteSpace(savedGuid))
              {
                value = outGuid;
              }
              else
              {
                Guid.TryParse(savedGuid, out outGuid);
                value = outGuid;
              }
            }
            else
            {
              throw new ArgumentException(string.Format("Value of type {0} is not supported.", value.GetType().Name));
            }

            break;
        }


        return null != value ? (T)value : defaultValue;
      }
    }

    /// <summary>
    /// Adds or updates the value 
    /// </summary>
    /// <param name="key">Key for settting</param>
    /// <param name="value">Value to set</param>
    /// <returns>True of was added or updated and you need to save it.</returns>
    public bool AddOrUpdateValue(string key, object value)
    {
      lock (locker)
      {
        Type typeOf = value.GetType();
        if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          typeOf = Nullable.GetUnderlyingType(typeOf);
        }
        var typeCode = Type.GetTypeCode(typeOf);
        var defaults = NSUserDefaults.StandardUserDefaults;
        switch (typeCode)
        {
          case TypeCode.Decimal:
            defaults.SetString(Convert.ToString(value), key);
            break;
          case TypeCode.Boolean:
            defaults.SetBool(Convert.ToBoolean(value), key);
            break;
          case TypeCode.Int64:
            defaults.SetString(Convert.ToString(value), key);
            break;
          case TypeCode.Double:
            defaults.SetDouble(Convert.ToDouble(value), key);
            break;
          case TypeCode.String:
            defaults.SetString(Convert.ToString(value), key);
            break;
          case TypeCode.Int32:
            defaults.SetInt(Convert.ToInt32(value), key);
            break;
          case TypeCode.Single:
            defaults.SetFloat(Convert.ToSingle(value), key);
            break;
          case TypeCode.DateTime:
            defaults.SetString(Convert.ToString(((DateTime)(object)value).Ticks), key);
            break;
          default:
            if (value is Guid)
            {
              defaults.SetString(((Guid)value).ToString(), key);
            }
            else
            {
              throw new ArgumentException(string.Format("Value of type {0} is not supported.", value.GetType().Name));
            }
            break;
        }
        try
        {
            defaults.Synchronize();
          
        }
        catch (Exception ex)
        {
          Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
        }
      }

     
      return true;
    }

    /// <summary>
    /// Saves all currents settings outs.
    /// </summary>
    [Obsolete("Save is deprecated and settings are automatically saved when AddOrUpdateValue is called.")]
    public void Save()
    {
     
    }

  }

}
