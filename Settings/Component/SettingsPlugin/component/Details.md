# Settings Plugin Details


Create and access settings from shared code across all of your mobile apps!

### Uses the native settings management
* Android: SharedPreferences
* iOS: NSUserDefaults
* Windows Phone: IsolatedStorageSettings
* Windows Store / Windows Phone RT: ApplicationDataContainer


### Example
```
private static ISettings AppSettings
{
  get
  {
    return CrossSettings.Current;
  }
}

private const string UserNameKey = "username_key";
private static readonly string UserNameDefault = string.Empty;

public static string UserName
{
  get { return AppSettings.GetValueOrDefault(UserNameKey, UserNameDefault); }
  set { AppSettings.AddOrUpdateValue(UserNameKey, value); }
}
```

Works completely from a shared code or PCL project.