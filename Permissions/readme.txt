Permissions Readme

Changelog:
[1.1.7]
-Fix issue for Windows Config
[1.1.6]
-Fix for checking photos permission on iOS #192
[1.1.5]
  -Fix odd instance where can't check manifest because current activity is null, now use application context.
  -If Activity is null when requesting then go ahead and return list of unknown permissions
[1.1.4]
  -Fix for #169, odd freeze for certain iOS permissions when requesting a second time in a denied state.
[1.1.3]
  -Remove help file, but add readme.txt
[1.1.1]
  -Fix odd edgecase in Android for nothing in Manifest.
  -Use Context instead of Activity when checking permisson if we can.
  -Change to params of permissions when requesting permissions.

Learn More:
http://www.github.com/jamesmontemagno/Xamarn.Plugins
http://www.xamarin.com/plugins

Created by James Montemagno:
http://twitter.com/jamesmontemagno
http://www.motzcod.es

**IMPORTANT**
Android:
It is required that you add the following override to any Activity that you will be requesting permissions from:

public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
{
    Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}

Additionally, Plugin.CurrentActivity was installed to propogate the current Activity up to this plugin. Please ensure that your Application class is correct configured.

