# Getting Started with Permissions Plugin

### API Usage

Call **CrossPermissions.Current** from any project or PCL to gain access to APIs.

**Should show request rationale**
```csharp
/// <summary>
/// Request to see if you should show a rationale for requesting permission
/// Only on Android
/// </summary>
/// <returns>True or false to show rationale</returns>
/// <param name="permission">Permission to check.</param>
Task<bool> ShouldShowRequestPermissionRationale(Permission permission);
```

**CheckPermissiontStatus**
```csharp
/// <summary>
/// Determines whether this instance has permission the specified permission.
/// </summary>
/// <returns><c>true</c> if this instance has permission the specified permission; otherwise, <c>false</c>.</returns>
/// <param name="permission">Permission to check.</param>
Task<PermissionStatus> CheckPermissionStatus(Permission permission);
```

**RequestPermissions**
```csharp
/// <summary>
/// Requests the permissions from the users
/// </summary>
/// <returns>The permissions and their status.</returns>
/// <param name="permissions">Permissions to request.</param>
Task<Dictionary<Permission, PermissionStatus>> RequestPermissions(params Permission[] permissions);
```

### In Action
Here is how you may use it with the Geolocator Plugin:

```csharp
try
{
    var status = await CrossPermissions.Current.CheckPermissionStatus(Permission.Location);
    if (status != PermissionStatus.Granted)
    {
        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationale(Permission.Location))
        {
            await DisplayAlert("Need location", "Gunna need that location", "OK");
        }

        var results = await CrossPermissions.Current.RequestPermissions(new[] {Permission.Location});
        status = results[Permission.Location];
    }

    if (status == PermissionStatus.Granted)
    {
        var results = await CrossGeolocator.Current.GetPositionAsync(10000);
        LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
    }
    else if (status != PermissionStatus.Unknown)
    {
        await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
    }
}
catch (Exception ex)
{
    LabelGeolocation.Text = "Error: " + ex;
}
```

### Available Permissions
```csharp
/// <summary>
/// Permission group that can be requested
/// </summary>
public enum Permission
{
    /// <summary>
    /// The unknown permission only used for return type, never requested
    /// </summary>
    Unknown,
    /// <summary>
    /// Android: Calendar
    /// iOS: Calendar (Events)
    /// </summary>
    Calendar,
    /// <summary>
    /// Android: Camera
    /// iOS: Photos (Camera Roll and Camera)
    /// </summary>
    Camera,
    /// <summary>
    /// Android: Contacts
    /// iOS: AddressBook
    /// </summary>
    Contacts,
    /// <summary>
    /// Android: Fine and Coarse Location
    /// iOS: CoreLocation (Always and WhenInUse)
    /// </summary>
    Location,
    /// <summary>
    /// Android: Microphone
    /// iOS: Microphone
    /// </summary>
    Microphone,
    /// <summary>
    /// Android: Phone
    /// iOS: Nothing
    /// </summary>
    Phone,
    /// <summary>
    /// Android: Nothing
    /// iOS: Photos
    /// </summary>
    Photos,
    /// <summary>
    /// Android: Nothing
    /// iOS: Reminders
    /// </summary>
    Reminders,
    /// <summary>
    /// Android: Body Sensors
    /// iOS: CoreMotion
    /// </summary>
    Sensors,
    /// <summary>
    /// Android: Sms
    /// iOS: Nothing
    /// </summary>
    Sms,
    /// <summary>
    /// Android: External Storage
    /// iOS: Nothing
    /// </summary>
    Storage
}
```
Read more about android permissions: http://developer.android.com/guide/topics/security/permissions.html#normal-dangerous


### **IMPORTANT**
Android:
You still need to request the permissions in your AndroidManifest.xml. Also ensure your MainApplication.cs was setup correctly from the CurrentActivity Plugin.

#### Android specific in your BaseActivity or MainActivity (for Xamarin.Forms) add this code:
```csharp
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
{
    PermissionsImplementation.Current.OnRequestPermissionsAsyncResult(requestCode, permissions, grantResults);
}
```
