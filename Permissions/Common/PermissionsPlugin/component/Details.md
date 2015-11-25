# Permissions Plugin Details

Simple cross platform plugin to check connection status of mobile device, gather connection type, bandwidths, and more. 

### Features
* Check Permission Status
* Request Permission

**Supports**
* Xamarin.iOS (x64 Unified) (iOS 7+)
* Xamarin.Android (API 14+)

**Blank Implementations for**
* Windows Phone 8 (Silverlight)
* Windows Ptone 8.1 RT
* Windows Store 8.1

Works completely from shared code or PCL projects.

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

Want to read about the creation, checkout my [in-depth blog post](http://motzcod.es/post/133939517717/simplified-ios-android-runtime-permissions-with).

