using Connectivity.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.App;
using Java.Net;
using System.Net.NetworkInformation;


namespace Connectivity.Plugin
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class ConnectivityImplementation : IConnectivity
  {
    private ConnectivityManager connectivityManager;
    private WifiManager wifiManager;

    protected ConnectivityManager ConnectivityManager
    {
      get
      {
        connectivityManager = connectivityManager ??
                               (ConnectivityManager)
                               (Application.Context
                                   .GetSystemService(Context.ConnectivityService));
        return connectivityManager;
      }
    }

    protected WifiManager WifiManager
    {
      get
      {
        wifiManager = wifiManager ??
                       (WifiManager)
                       (Application.Context.GetSystemService(Context.WifiService));
        return wifiManager;
      }
    }

    public bool IsConnected
    {
      get
      {
        try
        {
          var activeConnection = ConnectivityManager.ActiveNetworkInfo;

          return ((activeConnection != null) && activeConnection.IsConnected);
        }
        catch (Exception e)
        {
          Debug.WriteLine("Unable to get connected state - do you have ACCESS_NETWORK_STATE permission? - error: {0}", e);
          return false;
        }
      }
    }

    public async Task<bool> IsReachable(string host, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;

      return await Task.Run(() =>
      {
        bool reachable;
        try
        {
          reachable = InetAddress.GetByName(host).IsReachable(msTimeout);
        }
        catch (UnknownHostException)
        {
          reachable = false;
        }
        return reachable;
      });
     
    }

    public async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
    {

      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;

      return await Task.Run(async () =>
      {
        var sockaddr = new InetSocketAddress(host, port);
        using (var sock = new Socket())
        {
          try
          {
            await sock.ConnectAsync(sockaddr, msTimeout);
            return true;
          }
          catch (Exception)
          {
            return false;
          }
        }
      });
    }

    public IEnumerable<ConnectionType> ConnectionTypes
    {
      get
      {
        ConnectionType type;
        var activeConnection = ConnectivityManager.ActiveNetworkInfo;
        switch (activeConnection.Type)
        {
          case ConnectivityType.Wimax:
            type = ConnectionType.Wimax;
            break;
          case ConnectivityType.Wifi:
            type = ConnectionType.WiFi;
            break;
          default:
            type = ConnectionType.Cellular;
            break;
        }
        yield return type;
      }
    }

    public IEnumerable<UInt64> Bandwidths
    {
      get
      {
        try
        {
          if (ConnectionTypes.Contains(ConnectionType.WiFi))
            return new []{(UInt64)WifiManager.ConnectionInfo.LinkSpeed};
        }
        catch(Exception e)
        {
          Debug.WriteLine("Unable to get connected state - do you have ACCESS_WIFI_STATE permission? - error: {0}", e);
        }

        return new UInt64[] { };
      }
    }
  }
}