# Getting Started with Device Info Plugin


### API Usage

Call **CrossDeviceInfo.Current** from any project or PCL to gain access to APIs.

**GenerateAppId**
Used to generate a unique Id for your app.

```
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
```

**Id**
```
/// <summary>
/// This is the device specific Id (remember the correct permissions in your app to use this)
/// </summary>
string Id { get; }
```
Important:

Windows Phone:
Permissions to add:
ID_CAP_IDENTITY_DEVICE

**Device Model**
```
/// <summary>
/// Get the model of the device
/// </summary>
string Model { get; }
```


**Version**
```
/// <summary>
/// Get the version of the Operating System
/// </summary>
string Version { get; }
```

Returns the specific version number of the OS such as:
* iOS: 8.1
* Android: 4.4.4
* Windows Phone: 8.10.14219.0
* WinRT: always 8.1 until there is a work around

**Platform**
```
/// <summary>
/// Get the platform of the device
/// </summary>
Platform Platform { get; }
```

Returns the Platform Enum of:
```
public enum Platform
{
  Android,
  iOS,
  WindowsPhone,
  Windows
}
```
