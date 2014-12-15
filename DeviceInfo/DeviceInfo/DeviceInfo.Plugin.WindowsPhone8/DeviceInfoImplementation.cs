using DeviceInfo.Plugin.Abstractions;
using Microsoft.Phone.Info;
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
        object uniqueId;
        if (!DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
          return "";
        var bId = (byte[])uniqueId;
        return Convert.ToBase64String(bId);
      }
    }

    public string Model
    {
      get
      {
        object model;
        if (!DeviceExtendedProperties.TryGetValue("DeviceName", out model))
          return string.Empty;
        return model as string;
      }
    }

    public string Version
    {
      get { return Environment.OSVersion.Version.ToString(); }
    }

    public Platform Platform
    {
      get { return Platform.WindowsPhone; }
    }
  }
}