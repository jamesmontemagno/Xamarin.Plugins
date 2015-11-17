## Access Android Context and ViewControllers

This is a sample plugin of how to use the application context to start a new activity and also how to get access to the current UIViewController in iOS:


**Android**
Use the ApplicationContext with special flags

```csharp
//these flags are required when using application context
smsIntent.SetFlags(ActivityFlags.ClearTop);
smsIntent.SetFlags(ActivityFlags.NewTask);
Android.App.Application.Context.StartActivity(smsIntent);
```


**iOS**
Use the SharedApplication to gain access:

```csharp
UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(smsController, true);
```

#### License
Licensed under main repo license
