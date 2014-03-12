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
using MonoTouch.Foundation;
using Refractored.Xam.Settings.Abstractions;

namespace Refractored.Xam.Settings
{
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
          case TypeCode.Boolean:
            value = defaults.BoolForKey(key);
            break;
          case TypeCode.Int64:
            var savedval = defaults.StringForKey(key);
            value = Convert.ToInt64(savedval);
            break;
          case TypeCode.Double:
            value = defaults.DoubleForKey(key);
            break;
          case TypeCode.String:
            value = defaults.StringForKey(key);
            break;
          case TypeCode.Int32:
            value = defaults.IntForKey(key);
            break;
          case TypeCode.Single:
            value = defaults.FloatForKey(key);
            break;

          case TypeCode.DateTime:
            var savedTime = defaults.StringForKey(key);
            var ticks = string.IsNullOrWhiteSpace(savedTime) ? -1 : Convert.ToInt64(savedTime);
            if (ticks == -1)
              value = defaultValue;
            else
              value = new DateTime(ticks);
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
        }
      }

      return true;
    }

    /// <summary>
    /// Saves all currents settings outs.
    /// </summary>
    public void Save()
    {
      try
      {
        lock (locker)
        {
          var defaults = NSUserDefaults.StandardUserDefaults;
          defaults.Synchronize();
        }
      }
      catch (Exception)
      {
        //TODO: log stuff here
      }
    }

  }

}
