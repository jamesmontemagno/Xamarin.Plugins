using DeviceInfo.Plugin.Abstractions;
#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif
using System;


namespace DeviceInfo.Plugin
{
  /// <summary>
  /// Implementation for DeviceInfo
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
      get
      { 
        // iOS 6 and up
        return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
      }
    }

    public string Model
    {
      get { return UIDevice.CurrentDevice.Model; }
    }

    public string Version
    {
      get { return UIDevice.CurrentDevice.SystemVersion; }
    }


    public Platform Platform
    {
      get { return Platform.iOS; }
    }
  }
}