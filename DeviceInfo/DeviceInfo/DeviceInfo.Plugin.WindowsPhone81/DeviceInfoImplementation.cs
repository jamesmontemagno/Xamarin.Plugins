/*
 * Ported with permission from: Thomasz Cielecki @Cheesebaron
 * AppId: https://github.com/Cheesebaron/Cheesebaron.MvxPlugins
 */
 //---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------
using Plugin.DeviceInfo.Abstractions;
using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.System;
using Windows.System.Profile;
using System.Linq;
using Windows.UI.Xaml.Controls;
using System.Text.RegularExpressions;
using Windows.Security.ExchangeActiveSyncProvisioning;


namespace Plugin.DeviceInfo
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
    /// <inheritdoc/>
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
    /// <inheritdoc/>
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
    /// <inheritdoc/>
    public string Model
    {
      get { return deviceInfo.SystemProductName; }
    }
    /// <inheritdoc/>
    public string Version
    {
      get 
      {
        return "8.1";//Fix in future, not real great way to get this info in 8.1 
      }
    }
    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public Version VersionNumber
    {
        get
        {
            try
            {
                return new Version(Version);
            }
            catch
            {
                return new Version(8, 1);
            }
        }
    }
  }
}
