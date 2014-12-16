using Connectivity.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace Connectivity.Plugin
{
  /// <summary>
  /// Implementation for Connectivity
  /// </summary>
  public class ConnectivityImplementation : IConnectivity
  {

    public ConnectivityImplementation()
    {
      Bandwidths = new List<UInt64>();
      UpdateConnected();
      Reachability.ReachabilityChanged += (sender, args) => UpdateConnected();
    }



    private void UpdateConnected()
    {
      var remoteHostStatus = Reachability.RemoteHostStatus();
      var internetStatus = Reachability.InternetConnectionStatus();
      var localWifiStatus = Reachability.LocalWifiConnectionStatus();
      IsConnected = (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                      internetStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
                    (localWifiStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                      localWifiStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
                    (remoteHostStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                      remoteHostStatus == NetworkStatus.ReachableViaWiFiNetwork);
    }


    public bool IsConnected { get; private set; }

    public async Task<bool> IsReachable(string host, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;
      
      try
      {
        using(var ping = new Ping())
        {
          var reply = await ping.SendPingAsync(host, msTimeout);
          return (reply.Status == IPStatus.Success);
        }
      }
      catch(Exception ex)
      {
        return false;
      }
    }

    public async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;

      return await Task.Run(() =>
      {
        var clientDone = new ManualResetEvent(false);
        var reachable = false;
        var hostEntry = new DnsEndPoint(host, port);
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
          var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = hostEntry };
          socketEventArg.Completed += (s, e) =>
          {
            reachable = e.SocketError == SocketError.Success;

            clientDone.Set();
          };

          clientDone.Reset();

          socket.ConnectAsync(socketEventArg);

          clientDone.WaitOne(msTimeout);

          return reachable;
        }
      });
    }

    public IEnumerable<ConnectionType> ConnectionTypes 
    {
      get
      {
        var status = Reachability.InternetConnectionStatus();
        switch (status)
        {
          case NetworkStatus.ReachableViaCarrierDataNetwork:
            yield return ConnectionType.Cellular;
            break;
          case NetworkStatus.ReachableViaWiFiNetwork:
            yield return ConnectionType.WiFi;
            break;
          default:
            yield return ConnectionType.Other;
            break;
        }
      }
    }
    /// <summary>
    /// Not supported on iOS
    /// </summary>
    public IEnumerable<UInt64> Bandwidths
    {
      get;
      private set;
    }

  }
}