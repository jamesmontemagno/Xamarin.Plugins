using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/*
 * Ported with permission from: Thomasz Cielecki @Cheesebaron
 * Connectivity: https://github.com/Cheesebaron/Cheesebaron.MvxPlugins
 */
namespace Connectivity.Plugin.Abstractions
{
  /// <summary>
  /// Interface for Connectivity
  /// </summary>
  public interface IConnectivity
  {
    bool IsConnected { get; }
    Task<bool> IsReachable(string host, int msTimeout = 5000);
    Task<bool> IsPortReachable(string host, int port = 80, int msTimeout = 5000);
    IEnumerable<ConnectionType> ConnectionTypes { get; }
    IEnumerable<UInt64> Bandwidths { get; }
  }
}
