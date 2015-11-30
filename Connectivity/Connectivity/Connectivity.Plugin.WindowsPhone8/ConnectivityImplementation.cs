using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
using Plugin.Connectivity.Abstractions;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using System.Windows;

namespace Plugin.Connectivity
{
    /// <summary>
    /// Implementation for WinPhone 8
    /// </summary>
    public class ConnectivityImplementation : BaseConnectivity
    {
        bool isConnected;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConnectivityImplementation()
        {
            isConnected = IsConnected;
            //Must register for both for WP8 for some reason.
            DeviceNetworkInformation.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkAddressChanged;
        }

        void NetworkAddressChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        void NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            var previous = isConnected;
            var newConnected = IsConnected;

            if (previous == newConnected)
                return;
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                OnConnectivityChanged(new ConnectivityChangedEventArgs { IsConnected = newConnected });
            });
        
        }

        /// <summary>
        /// Gets if there is an active internet connection
        /// </summary>
        public override bool IsConnected =>
               (isConnected = NetworkInformation.GetInternetConnectionProfile()?.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);

        /// <summary>
        /// Tests if a host name is pingable
        /// </summary>
        /// <param name="host">The host name can either be a machine name, such as "java.sun.com", or a textual representation of its IP address (127.0.0.1)</param>
        /// <param name="msTimeout">Timeout in milliseconds</param>
        /// <returns></returns>
        public override async Task<bool> IsReachable(string host, int msTimeout = 5000)
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

        /// <summary>
        /// Tests if a remote host name is reachable
        /// </summary>
        /// <param name="host">Host name can be a remote IP or URL of website (no http:// or www.)</param>
        /// <param name="port">Port to attempt to check is reachable.</param>
        /// <param name="msTimeout">Timeout in milliseconds.</param>
        /// <returns></returns>
        public override async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host");

            if (!IsConnected)
                return false;

            host = host.Replace("http://www.", string.Empty).
               Replace("http://", string.Empty).
               Replace("https://www.", string.Empty).
               Replace("https://", string.Empty);

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
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to reach: " + host + " Error: " + ex);
                    return false;
                }
            });
        }

        /// <summary>
        /// Returns connection types active
        /// </summary>
        public override IEnumerable<ConnectionType> ConnectionTypes
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

        /// <summary>
        /// Returns list of Bandwidths
        /// </summary>
        public override IEnumerable<UInt64> Bandwidths
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
        private bool disposed = false;
        /// <summary>
        /// Dispose of class
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DeviceNetworkInformation.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
                    NetworkChange.NetworkAddressChanged -= NetworkAddressChanged;
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
