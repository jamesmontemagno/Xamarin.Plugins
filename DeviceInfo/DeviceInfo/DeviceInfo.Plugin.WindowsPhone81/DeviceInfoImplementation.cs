using DeviceInfo.Plugin.Abstractions;
using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.System;
using Windows.System.Profile;
using System.Linq;
using Windows.UI.Xaml.Controls;
using System.Text.RegularExpressions;
using Windows.Security.ExchangeActiveSyncProvisioning;


namespace DeviceInfo.Plugin
{
  /// <summary>
  /// Implementation for DeviceInfo
  /// </summary>
  public class DeviceInfoImplementation : IDeviceInfo
  {

    EasClientDeviceInformation deviceInfo;
    public DeviceInfoImplementation()
    {
      deviceInfo = new EasClientDeviceInformation();
    }

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
        var token = HardwareIdentification.GetPackageSpecificToken(null);
        var hardwareId = token.Id;
        var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

        var bytes = new byte[hardwareId.Length];
        dataReader.ReadBytes(bytes);

        return Convert.ToBase64String(bytes);
      }
    }

    public string Model
    {
      get { return deviceInfo.SystemProductName; }
    }

    public string Version
    {
      get 
      {
        return "8.1";//Fix in future, not real great way to get this info in 8.1 
      }
    }

    public Platform Platform
    {
      get 
      {
#if WINDOWS_APP
        return Platform.Windows;
#else
        return Platform.WindowsPhone; 
#endif
      }
    }
  }
}