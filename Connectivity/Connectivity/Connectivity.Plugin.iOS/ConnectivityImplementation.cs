using Connectivity.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
  public class ConnectivityImplementation : BaseConnectivity
  {

    public ConnectivityImplementation()
    {
      UpdateConnected(false);
      Reachability.ReachabilityChanged += (sender, args) => UpdateConnected();
    }


    private bool isConnected;
    private void UpdateConnected(bool triggerChange = true)
    {
      var remoteHostStatus = Reachability.RemoteHostStatus();
      var internetStatus = Reachability.InternetConnectionStatus();
      var localWifiStatus = Reachability.LocalWifiConnectionStatus();

      var previous = isConnected;
      isConnected = (internetStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                      internetStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
                    (localWifiStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                      localWifiStatus == NetworkStatus.ReachableViaWiFiNetwork) ||
                    (remoteHostStatus == NetworkStatus.ReachableViaCarrierDataNetwork ||
                      remoteHostStatus == NetworkStatus.ReachableViaWiFiNetwork);

      //dont' trigger on 
      if(triggerChange && previous != isConnected)
        OnConnectivityChanged(new ConnectivityChangedEventArgs { IsConnected = isConnected });
    }


    public override bool IsConnected { get { return isConnected; } }

    public override async Task<bool> IsReachable(string host, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;

      return await IsRemoteReachable(host, 80, msTimeout);
    }

    public override async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;

      return await Task.Run(() =>
      {
        try
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
        }
        catch(Exception ex)
        {
          Debug.WriteLine("Unable to reach: " + host + " Error: " + ex);
          return false;
        }
      });
    }

    public override IEnumerable<ConnectionType> ConnectionTypes
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
    public override IEnumerable<UInt64> Bandwidths
    {
      get { return new UInt64[] { }; }
    }

  }
}