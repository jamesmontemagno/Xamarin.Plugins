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
    /// <summary>
    /// Gets if there is an active internet connection
    /// </summary>
    bool IsConnected { get; }
    /// <summary>
    /// Tests if a host name is pingable
    /// </summary>
    /// <param name="host">The host name can either be a machine name, such as "java.sun.com", or a textual representation of its IP address (127.0.0.1)</param>
    /// <param name="msTimeout">Timeout in milliseconds</param>
    /// <returns></returns>
    Task<bool> IsReachable(string host, int msTimeout = 5000);
    /// <summary>
    /// Tests if a remote host name is reachable
    /// </summary>
    /// <param name="host">Host name can be a remote IP or URL of website</param>
    /// <param name="port">Port to attempt to check is reachable.</param>
    /// <param name="msTimeout">Timeout in milliseconds.</param>
    /// <returns></returns>
    Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000);
    /// <summary>
    /// Gets the list of all active connection types.
    /// </summary>
    IEnumerable<ConnectionType> ConnectionTypes { get; }
    /// <summary>
    /// Retrieves a list of available bandwidths for the platform.
    /// Only active connections.
    /// </summary>
    IEnumerable<UInt64> Bandwidths { get; }
  }
}
