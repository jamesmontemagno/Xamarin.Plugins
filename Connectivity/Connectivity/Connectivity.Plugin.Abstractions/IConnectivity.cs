using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/*
 * Ported with permission from: Thomasz Cielecki @Cheesebaron
 * Connectivity: https://github.com/Cheesebaron/Cheesebaron.MvxPlugins
 */
namespace Plugin.Connectivity.Abstractions
{
    /// <summary>
    /// Interface for Connectivity
    /// </summary>
    public interface IConnectivity : IDisposable
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

        /// <summary>
        /// Event handler when connection changes
        /// </summary>
        event ConnectivityChangedEventHandler ConnectivityChanged;
    }

    /// <summary>
    /// Arguments to pass to event handlers
    /// </summary>
    public class ConnectivityChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets if there is an active internet connection
        /// </summary>
        public bool IsConnected { get; set; }
    }

    /// <summary>
    /// Connectivity changed event handlers
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ConnectivityChangedEventHandler(object sender, ConnectivityChangedEventArgs e);

}
