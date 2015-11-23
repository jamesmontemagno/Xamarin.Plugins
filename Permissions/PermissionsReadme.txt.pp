Permissions Readme
Find the most up to date information at: https://github.com/jamesmontemagno/Xamarin.Plugins

**IMPORTANT**
Android:
It is required that you add the following override to any Activity that you will be requesting permissions from:

public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
{
    Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}

Additionally, Plugin.CurrentActivity was installed to propogate the current Activity up to this plugin. Please ensure that your Application class is correct configured.

