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

namespace Connectivity.Plugin
{
  public class ConnectivityImplementation : IConnectivity
  {
    public bool IsConnected
    {
      get 
      {
        return NetworkInterface.GetIsNetworkAvailable();
      }
    }

    public async Task<bool> IsReachable(string host, int msTimeout = 5000)
    {
      return await IsPortReachable(host, 80, msTimeout);
    }

    public async Task<bool> IsPortReachable(string host, int port = 80, int msTimeout = 5000)
    {
      if (string.IsNullOrEmpty(host))
        throw new ArgumentNullException("host");

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
        return false;
      }
      finally
      {
      }
    }

    public IEnumerable<ConnectionType> ConnectionTypes
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

    public IEnumerable<UInt64> Bandwidths
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
  }
}