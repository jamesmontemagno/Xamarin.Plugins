using DeviceInfo.Plugin.Abstractions;
using System;
using Android.OS;


namespace DeviceInfo.Plugin
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class DeviceInfoImplementation : IDeviceInfo
  {
    public string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null)
    {
      var appId = "";

      if (!string.IsNullOrEmpty(prefix))
        appId += prefix;

      appId += Guid.NewGuid().ToString();

      if (usingPhoneId)
        appId += Id;

      if (!string.IsNullOrEmpty(suffix))
        appId += suffix;

      return appId;
    }

    public string Id
    {
      get { return Build.Serial; }
    }

    public string Model
    {
      get { return Build.Model; }
    }

    public string Version
    {
      get { return Build.VERSION.Release; }
    }


    public Platform Platform
    {
      get { return Platform.Android; }
    }
  }
}