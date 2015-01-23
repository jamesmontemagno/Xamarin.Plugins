

using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Refractored.Xam.Settings.Abstractions;

namespace Refractored.Xam.Settings
{
  /// <summary>
  /// Main Implementation for ISettings
  /// </summary>
  public class Settings : ISettings
  {
    private static ISharedPreferences SharedPreferences { get; set; }
    private static ISharedPreferencesEditor SharedPreferencesEditor { get; set; }
    private readonly object locker = new object();

    /// <summary>
    /// Main Constructor
    /// </summary>
    public Settings()
    {
      SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
      SharedPreferencesEditor = SharedPreferences.Edit();

    }

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
        Type typeOf = typeof(T);
        if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
          typeOf = Nullable.GetUnderlyingType(typeOf);
        }


        object value = null;
        var typeCode = Type.GetTypeCode(typeOf);
        bool resave = false;
        switch (typeCode)
        {
          case TypeCode.Decimal:
            //Android doesn't have decimal in shared prefs so get string and convert
            var savedDecimal = string.Empty;
            try
            {
              savedDecimal = SharedPreferences.GetString(key, string.Empty);
            }
            catch(Java.Lang.ClassCastException cce)
            {
              Console.WriteLine("Settings 2.0 change, have to remove key.");
            
              try
              {
                Console.WriteLine("Attempting to get old value.");
                savedDecimal = SharedPreferences.GetLong(key, (long)Convert.ToDecimal(defaultValue, System.Globalization.CultureInfo.InvariantCulture)).ToString();
                Console.WriteLine("Old value has been parsed and will be updated and saved.");
              }
              catch (Java.Lang.ClassCastException cce2)
              {
                Console.WriteLine("Could not parse old value, will be lost.");
              }
              Remove("key");
              resave = true;
            }
            if (string.IsNullOrWhiteSpace(savedDecimal))
              value = Convert.ToDecimal(defaultValue, System.Globalization.CultureInfo.InvariantCulture);
            else
              value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);
            
            if (resave)
              AddOrUpdateValue(key, value);

            break;
          case TypeCode.Boolean:
            value = SharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
            break;
          case TypeCode.Int64:
            value = (Int64)SharedPreferences.GetLong(key, (long)Convert.ToInt64(defaultValue, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.String:

            value = SharedPreferences.GetString(key, Convert.ToString(defaultValue));
            break;
          case TypeCode.Double:
            //Android doesn't have double, so must get as string and parse.
            var savedDouble = string.Empty;
            try
            {
              savedDouble = SharedPreferences.GetString(key, string.Empty);
            }
            catch(Java.Lang.ClassCastException cce)
            {
              Console.WriteLine("Settings 2.0  change, have to remove key.");

              try
              {
                Console.WriteLine("Attempting to get old value.");
                savedDouble = SharedPreferences.GetLong(key, (long)Convert.ToDouble(defaultValue, System.Globalization.CultureInfo.InvariantCulture)).ToString();
                Console.WriteLine("Old value has been parsed and will be updated and saved.");
              }
              catch (Java.Lang.ClassCastException cce2)
              {
                Console.WriteLine("Could not parse old value, will be lost.");
              }
              Remove(key);
              resave = true;
            }
            if (string.IsNullOrWhiteSpace(savedDouble))
              value = Convert.ToDouble(defaultValue, System.Globalization.CultureInfo.InvariantCulture);
            else
              value = Convert.ToDouble(savedDouble, System.Globalization.CultureInfo.InvariantCulture);
            
            if (resave)
              AddOrUpdateValue(key, value);
            break;
          case TypeCode.Int32:
            value = SharedPreferences.GetInt(key, Convert.ToInt32(defaultValue, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.Single:
            value = SharedPreferences.GetFloat(key, Convert.ToSingle(defaultValue, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.DateTime:
            var ticks = SharedPreferences.GetLong(key, -1);
            if (ticks == -1)
              value = defaultValue;
            else
              value = new DateTime(ticks);
            break;
          default:

            if (defaultValue is Guid)
            {
              var outGuid = Guid.Empty;
              Guid.TryParse(SharedPreferences.GetString(key, Guid.Empty.ToString()), out outGuid);
              value = outGuid;
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
    /// Adds or updates a value
    /// </summary>
    /// <param name="key">key to update</param>
    /// <param name="value">value to set</param>
    /// <returns>True if added or update and you need to save</returns>
    public bool AddOrUpdateValue<T>(string key, T value)
    {
      Type typeOf = typeof(T);
      if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        typeOf = Nullable.GetUnderlyingType(typeOf);
      }
      var typeCode = Type.GetTypeCode(typeOf);
      return AddOrUpdateValue(key, value, typeCode);
    }


    /// <summary>
    /// Adds or updates a value
    /// </summary>
    /// <param name="key">key to update</param>
    /// <param name="value">value to set</param>
    /// <returns>True if added or update and you need to save</returns>
    /// <exception cref="NullReferenceException">If value is null, this will be thrown.</exception>
    [Obsolete("This method is now obsolete, please use generic version as this may be removed in the future.")]
    public bool AddOrUpdateValue(string key, object value)
    {
      Type typeOf = value.GetType();
      if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        typeOf = Nullable.GetUnderlyingType(typeOf);
      }
      var typeCode = Type.GetTypeCode(typeOf);
      return AddOrUpdateValue(key, value, typeCode);
    }

    private bool AddOrUpdateValue(string key, object value, TypeCode typeCode)
    {
      lock(locker)
      {
        switch (typeCode)
        {
          case TypeCode.Decimal:
            SharedPreferencesEditor.PutString(key, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.Boolean:
            SharedPreferencesEditor.PutBoolean(key, Convert.ToBoolean(value));
            break;
          case TypeCode.Int64:
            SharedPreferencesEditor.PutLong(key, (long)Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.String:
            SharedPreferencesEditor.PutString(key, Convert.ToString(value));
            break;
          case TypeCode.Double:
            SharedPreferencesEditor.PutString(key, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.Int32:
            SharedPreferencesEditor.PutInt(key, Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.Single:
            SharedPreferencesEditor.PutFloat(key, Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture));
            break;
          case TypeCode.DateTime:
            SharedPreferencesEditor.PutLong(key, (Convert.ToDateTime(value)).Ticks);
            break;
          default:
            if(value is Guid)
            {
              if(value == null)
                value = Guid.Empty;

              SharedPreferencesEditor.PutString(key, ((Guid)value).ToString());
            }
            else
            {
              throw new ArgumentException(string.Format("Value of type {0} is not supported.", value.GetType().Name));
            }
            break;
        }

        SharedPreferencesEditor.Commit();

      }

      return true;
    }

    /// <summary>
    /// Saves out all current settings
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
        SharedPreferencesEditor.Remove(key);
        SharedPreferencesEditor.Commit();
      }
    }
  }
}
