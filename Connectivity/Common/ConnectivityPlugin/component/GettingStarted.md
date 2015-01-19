# Getting Started with Connectivity Plugin




### API Usage

Call **CrossConnectivity.Current** from any project or PCL to gain access to APIs.


**IsConnected**
```
/// <summary>
/// Gets if there is an active internet connection
/// </summary>
bool IsConnected { get; }
```

**ConnectionTypes**
```
/// <summary>
/// Gets the list of all active connection types.
/// </summary>
IEnumerable<ConnectionType> ConnectionTypes { get; }
```

**Bandwidths**
```
/// <summary>
/// Retrieves a list of available bandwidths for the platform.
/// Only active connections.
/// </summary>
IEnumerable<UInt64> Bandwidths { get; }
```

#### Pinging Hosts

**IsReachable**
```
/// <summary>
/// Tests if a host name is pingable
/// </summary>
/// <param name="host">The host name can either be a machine name, such as "java.sun.com", or a textual representation of its IP address (127.0.0.1)</param>
/// <param name="msTimeout">Timeout in milliseconds</param>
/// <returns></returns>
Task<bool> IsReachable(string host, int msTimeout = 5000);
```

**IsRemoteReachable**
```
/// <summary>
/// Tests if a remote host name is reachable
/// </summary>
/// <param name="host">Host name can be a remote IP or URL of website</param>
/// <param name="port">Port to attempt to check is reachable.</param>
/// <param name="msTimeout">Timeout in milliseconds.</param>
/// <returns></returns>
Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000);
```

#### Changes in Connectivity
When any network connectiivty is gained, changed, or loss you can register for an event to fire:
```
/// <summary>
/// Event handler when connection changes
/// </summary>
event ConnectivityChangedEventHandler ConnectivityChanged; 
```

You will get a ConnectivityChangeEventArgs with the status if you are connected or not:
```
public class ConnectivityChangedEventArgs : EventArgs
{
  public bool IsConnected { get; set; }
}

public delegate void ConnectivityChangedEventHandler(object sender, ConnectivityChangedEventArgs e);
```

Usage sample from Xamarin.Forms:
```
CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
  {
    page.DisplayAlert("Connectivity Changed", "IsConnected: " + args.IsConnected.ToString(), "OK");
  };
```


### **IMPORTANT**
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
