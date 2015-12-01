using Plugin.DeviceInfo.Abstractions;
using System;
using Windows.System.Profile;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Foundation.Metadata;

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
                if (ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
                {
                    var token = HardwareIdentification.GetPackageSpecificToken(null);
                    var hardwareId = token.Id;
                    var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                    var bytes = new byte[hardwareId.Length];
                    dataReader.ReadBytes(bytes);

                    return Convert.ToBase64String(bytes);
                }
                else
                {
                    return "unsupported";
                }
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
                return AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            }
        }
        /// <inheritdoc/>
        public Abstractions.Platform Platform
        {
            get
            {
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
                    return Abstractions.Platform.Windows;
                else
                    return Abstractions.Platform.WindowsPhone;
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
                    return new Version(10, 0);
                }
            }
        }
    }
}
