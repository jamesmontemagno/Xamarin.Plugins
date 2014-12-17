Connectivity Readme
Find the most up to date information at: https://github.com/jamesmontemagno/Xamarin.Plugins

**IMPORTANT**
Android:
You must request ACCESS_NETWORK_STATE permission to get the network state
You must request ACCESS_WIFI_STATE to get speeds

iOS:
Bandwidths is not supported and will always return an empty list.

Windows 8.1 & Windows Phone 8.1 RT:
RT apps can not perform loopback, so you can not use IsReachable to query the states of a local IP.

Permissions to think about:
The Private Networks (Client & Server) capability is represented by the Capability name = "privateNetworkClientServer" tag in the app manifest. 
The Internet (Client & Server) capability is represented by the Capability name = "internetClientServer" tag in the app manifest.
