using System;

/*
 * Ported with permission from: Thomasz Cielecki @Cheesebaron
 * AppId: https://github.com/Cheesebaron/Cheesebaron.MvxPlugins
 */

namespace DeviceInfo.Plugin.Abstractions
{
  /// <summary>
  /// Interface for DeviceInfo
  /// </summary>
  public interface IDeviceInfo
  {
    /// <summary>
    /// Generates a an AppId optionally using the PhoneId a prefix and a suffix and a Guid to ensure uniqueness
    /// 
    /// The AppId format is as follows {prefix}guid{phoneid}{suffix}, where parts in {} are optional.
    /// </summary>
    /// <param name="usingPhoneId">Setting this to true adds the device specific id to the AppId (remember to give the app the correct permissions)</param>
    /// <param name="prefix">Sets the prefix of the AppId</param>
    /// <param name="suffix">Sets the suffix of the AppId</param>
    /// <returns></returns>
    string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null);

    /// <summary>
    /// This is the device specific Id (remember the correct permissions in your app to use this)
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Get the model of the device
    /// </summary>
    string Model { get; }

    /// <summary>
    /// Get the version of the Operating System
    /// </summary>
    string Version { get; }


    /// <summary>
    /// Get the platform of the device
    /// </summary>
    Platform Platform { get; }
  }
}
