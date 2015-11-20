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
using Android.OS;


namespace Plugin.DeviceInfo
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class DeviceInfoImplementation : IDeviceInfo
  {
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
      get { return Build.Serial; }
    }
    /// <inheritdoc/>
    public string Model
    {
      get { return Build.Model; }
    }
    /// <inheritdoc/>
    public string Version
    {
      get { return Build.VERSION.Release; }
    }

    /// <inheritdoc/>
    public Platform Platform
    {
      get { return Platform.Android; }
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
                return new Version();
            }
        }
    }
  }
}
