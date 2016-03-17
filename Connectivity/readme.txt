Connectivity Readme

Change Log:
[2.1.2]
-Fix disconnected state in windows #228 and #244

[2.1.1]
-Fix Windows, occurances on Simulator or Device that network returns Local even though are online, now will return true for IsConnected.

[2.0.2]
-Fix when running a WP8SL app on an actual 8.0 device.

[2.0.1]
-Remove help file, add readme file
-Add blank .NET 4.5 implementation for unit testing
-Remove dependency that wasn't needed

[2.0.0]
-BREAKING CHANGE: New Namespace: Plugin.Connectivity
-Automatically add permissions for Android!
-Add UWP Support

Learn More:
http://www.github.com/jamesmontemagno/Xamarin.Plugins
http://www.xamarin.com/plugins

Created by James Montemagno:
http://twitter.com/jamesmontemagno
http://www.motzcod.es

**IMPORTANT**
Android:
The following persmissions are automatically added for you:
ACCESS_NETWORK_STATE & ACCESS_WIFI_STATE

iOS:
Bandwidths are not supported and will always return an empty list.

Windows 8.1 & Windows Phone 8.1 RT:
RT apps can not perform loopback, so you can not use IsReachable to query the states of a local IP.

Permissions to think about:
The Private Networks (Client & Server) capability is represented by the Capability name = "privateNetworkClientServer" tag in the app manifest. 
The Internet (Client & Server) capability is represented by the Capability name = "internetClientServer" tag in the app manifest.
