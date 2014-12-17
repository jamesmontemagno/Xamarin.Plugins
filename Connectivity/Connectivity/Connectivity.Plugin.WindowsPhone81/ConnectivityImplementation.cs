using System;
using System.Collections.Generic;
using System.Linq;
using Connectivity.Plugin.Abstractions;
using Windows.Networking.Connectivity;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;
using Windows.Networking.Sockets;
using Windows.Networking;
using System.Diagnostics;

namespace Connectivity.Plugin
{
  /// <summary>
  /// Connectivity Implemenation WinRT
  /// </summary>
  public class ConnectivityImplementation : BaseConnectivity
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public ConnectivityImplementation()
    {
      NetworkInformation.NetworkStatusChanged += NetworkStatusChanged;
    }

    void NetworkStatusChanged(object sender)
    {
      OnConnectivityChanged(new ConnectivityChangedEventArgs { IsConnected = IsConnected });  
    }
    /// <summary>
    /// Gets if there is an active internet connection
    /// </summary>
    public override bool IsConnected
    {
      get
      {
        return NetworkInterface.GetIsNetworkAvailable();
      }
    }

    /// <summary>
    /// Checks if remote is reachable. RT apps can not do loopback so this will alway return false.
    /// You can use it to check remote calls though.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="msTimeout"></param>
    /// <returns></returns>
    public override async Task<bool> IsReachable(string host, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;


     
      try
      {
        var serverHost = new HostName(host);
        using(var client = new StreamSocket())
        {
          await client.ConnectAsync(serverHost, "http");
          return true;
        }


      }
      catch(Exception ex)
      {
        Debug.WriteLine("Unable to reach: " + host + " Error: " + ex);
        return false;
      }
    }

    /// <summary>
    /// Tests if a remote host name is reachable
    /// </summary>
    /// <param name="host">Host name can be a remote IP or URL of website</param>
    /// <param name="port">Port to attempt to check is reachable.</param>
    /// <param name="msTimeout">Timeout in milliseconds.</param>
    /// <returns></returns>
    public override async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");
     
      if (!IsConnected)
        return false;

      try
      {
        using (var tcpClient = new StreamSocket())
        {
          await tcpClient.ConnectAsync(
              new Windows.Networking.HostName(host),
              port.ToString(),
              SocketProtectionLevel.PlainSocket);

          var localIp = tcpClient.Information.LocalAddress.DisplayName;
          var remoteIp = tcpClient.Information.RemoteAddress.DisplayName;

          tcpClient.Dispose();

          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Unable to reach: " + host + " Error: " + ex);
        return false;
      }
    }

    /// <summary>
    /// Gets the list of all active connection types.
    /// </summary>
    public override IEnumerable<ConnectionType> ConnectionTypes
    {
      get
      {
        var networkInterfaceList = NetworkInformation.GetConnectionProfiles();
        foreach (var networkInterfaceInfo in networkInterfaceList.Where(networkInterfaceInfo => networkInterfaceInfo.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None))
        {
          var type = ConnectionType.Other;

          if (networkInterfaceInfo.NetworkAdapter != null)
          {

            switch (networkInterfaceInfo.NetworkAdapter.IanaInterfaceType)
            {
              case 6:
                type = ConnectionType.Desktop;
                break;
              case 71:
                type = ConnectionType.WiFi;
                break;
              case 243:
              case 244:
                type = ConnectionType.Cellular;
                break;
            }
          }

          yield return type;
        }
      }
    }

    /// <summary>
    /// Retrieves a list of available bandwidths for the platform.
    /// Only active connections.
    /// </summary>
    public override IEnumerable<UInt64> Bandwidths
    {
      get
      {
        var networkInterfaceList = NetworkInformation.GetConnectionProfiles();
        foreach (var networkInterfaceInfo in networkInterfaceList.Where(networkInterfaceInfo => networkInterfaceInfo.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.None))
        {
          UInt64 speed = 0;

          if (networkInterfaceInfo.NetworkAdapter != null)
          {
            speed = (UInt64)networkInterfaceInfo.NetworkAdapter.OutboundMaxBitsPerSecond;
          }

          yield return speed;
        }
      }
    }
    private bool disposed = false;
    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    public override void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          NetworkInformation.NetworkStatusChanged -= NetworkStatusChanged;
        }

        disposed = true;
      }

      base.Dispose(disposing);
    }
  }
}