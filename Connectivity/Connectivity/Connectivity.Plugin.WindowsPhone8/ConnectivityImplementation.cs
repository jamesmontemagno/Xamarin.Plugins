using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
using Connectivity.Plugin.Abstractions;
using System.Diagnostics;

namespace Connectivity.Plugin
{
  public class ConnectivityImplementation : IConnectivity
  {
    public bool IsConnected
    {
      get { return DeviceNetworkInformation.IsNetworkAvailable; }
    }

    public async Task<bool> IsReachable(string host, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

      if (!IsConnected)
        return false;

      return await Task.Run(() =>
      {
        var manualResetEvent = new ManualResetEvent(false);
        var reachable = false;
        DeviceNetworkInformation.ResolveHostNameAsync(new DnsEndPoint(host, 80), result =>
        {
          reachable = result.NetworkInterface != null;
          manualResetEvent.Set();
        }, null);
        manualResetEvent.WaitOne(TimeSpan.FromMilliseconds(msTimeout));
        return reachable;
      });
    }

    public async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
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

    public IEnumerable<ConnectionType> ConnectionTypes
    {
      get
      {
        var networkInterfaceList = new NetworkInterfaceList();
        foreach (var networkInterfaceInfo in networkInterfaceList.Where(networkInterfaceInfo => networkInterfaceInfo.InterfaceState == ConnectState.Connected))
        {
          ConnectionType type;
          switch (networkInterfaceInfo.InterfaceSubtype)
          {
            case NetworkInterfaceSubType.Desktop_PassThru:
              type = ConnectionType.Desktop;
              break;
            case NetworkInterfaceSubType.WiFi:
              type = ConnectionType.WiFi;
              break;
            case NetworkInterfaceSubType.Unknown:
              type = ConnectionType.Other;
              break;
            default:
              type = ConnectionType.Cellular;
              break;
          }
          yield return type;
        }
      }
    }

    public IEnumerable<UInt64> Bandwidths
    {
      get
      {
        var networkInterfaceList = new NetworkInterfaceList();
        return
            networkInterfaceList.Where(
                networkInterfaceInfo => networkInterfaceInfo.InterfaceState == ConnectState.Connected
                && networkInterfaceInfo.InterfaceSubtype != NetworkInterfaceSubType.Unknown)
                                .Select(networkInterfaceInfo => (UInt64)networkInterfaceInfo.Bandwidth)
                                .ToArray();
      }
    }
  }
}