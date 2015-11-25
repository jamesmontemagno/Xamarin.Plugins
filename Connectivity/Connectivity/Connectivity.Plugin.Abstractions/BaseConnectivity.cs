using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Connectivity.Abstractions
{
    /// <summary>
    /// Base class for all connectivity classes
    /// </summary>
    public abstract class BaseConnectivity : IConnectivity, IDisposable
    {
        /// <summary>
        /// Gets if there is an active internet connection
        /// </summary>
        public abstract bool IsConnected
        {
            get;
        }

        /// <summary>
        /// Tests if a host name is pingable
        /// </summary>
        /// <param name="host">The host name can either be a machine name, such as "java.sun.com", or a textual representation of its IP address (127.0.0.1)</param>
        /// <param name="msTimeout">Timeout in milliseconds</param>
        /// <returns></returns>
        public abstract Task<bool> IsReachable(string host, int msTimeout = 5000);

        /// <summary>
        /// Tests if a remote host name is reachable
        /// </summary>
        /// <param name="host">Host name can be a remote IP or URL of website</param>
        /// <param name="port">Port to attempt to check is reachable.</param>
        /// <param name="msTimeout">Timeout in milliseconds.</param>
        /// <returns></returns>
        public abstract Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000);

        /// <summary>
        /// Gets the list of all active connection types.
        /// </summary>
        public abstract IEnumerable<ConnectionType> ConnectionTypes
        {
            get;
        }

        /// <summary>
        /// Retrieves a list of available bandwidths for the platform.
        /// Only active connections.
        /// </summary>
        public abstract IEnumerable<ulong> Bandwidths
        {
            get;
        }

        /// <summary>
        /// When connectivity changes
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnConnectivityChanged(ConnectivityChangedEventArgs e) =>
            ConnectivityChanged?.Invoke(this, e);
        

        /// <summary>
        /// Connectivity event
        /// </summary>
        public event ConnectivityChangedEventHandler ConnectivityChanged;


        /// <summary>
        /// Dispose of class and parent classes
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose up
        /// </summary>
        ~BaseConnectivity()
        {
            Dispose(false);
        }
        private bool disposed = false;
        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose only
                }

                disposed = true;
            }
        }
    }
}
