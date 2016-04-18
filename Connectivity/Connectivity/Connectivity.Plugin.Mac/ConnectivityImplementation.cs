using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace Plugin.Connectivity
{
	/// <summary>
	/// Implementation for Connectivity
	/// </summary>
	public class ConnectivityImplementation : BaseConnectivity
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ConnectivityImplementation ()
		{
			UpdateConnected (false);
			Reachability.ReachabilityChanged += ReachabilityChanged;
		}

		void ReachabilityChanged (object sender, EventArgs e)
		{
			UpdateConnected ();
		}


		private bool isConnected;
		private NetworkStatus previousInternetStatus = NetworkStatus.NotReachable;

		private void UpdateConnected (bool triggerChange = true)
		{
			var remoteHostStatus = Reachability.RemoteHostStatus ();
			var internetStatus = Reachability.InternetConnectionStatus ();
			var localWifiStatus = Reachability.LocalWifiConnectionStatus ();

			var previouslyConnected = isConnected;
			isConnected = internetStatus == NetworkStatus.ReachableViaWiFiNetwork ||
			localWifiStatus == NetworkStatus.ReachableViaWiFiNetwork ||
			remoteHostStatus == NetworkStatus.ReachableViaWiFiNetwork;

			if (triggerChange && (previouslyConnected != isConnected || previousInternetStatus != internetStatus))
				OnConnectivityChanged (new ConnectivityChangedEventArgs { IsConnected = isConnected });
			previousInternetStatus = internetStatus;
		}


		/// <summary>
		/// Gets if there is an active internet connection
		/// </summary>
		public override bool IsConnected { get { return isConnected; } }

		/// <summary>
		/// Tests if a host name is pingable
		/// </summary>
		/// <param name="host">The host name can either be a machine name, such as "java.sun.com", or a textual representation of its IP address (127.0.0.1)</param>
		/// <param name="msTimeout">Timeout in milliseconds</param>
		/// <returns></returns>
		public override async Task<bool> IsReachable (string host, int msTimeout = 5000)
		{
			if (string.IsNullOrEmpty (host))
				throw new ArgumentNullException ("host");

			if (!IsConnected)
				return false;

			return await IsRemoteReachable (host, 80, msTimeout);
		}

		/// <summary>
		/// Tests if a remote host name is reachable 
		/// </summary>
		/// <param name="host">Host name can be a remote IP or URL of website</param>
		/// <param name="port">Port to attempt to check is reachable.</param>
		/// <param name="msTimeout">Timeout in milliseconds.</param>
		/// <returns></returns>
		public override async Task<bool> IsRemoteReachable (string host, int port = 80, int msTimeout = 5000)
		{
			if (string.IsNullOrEmpty (host))
				throw new ArgumentNullException ("host");

			if (!IsConnected)
				return false;

			host = host.Replace ("http://www.", string.Empty).
              		Replace ("http://", string.Empty).
              		Replace ("https://www.", string.Empty).
              		Replace ("https://", string.Empty).
              		TrimEnd('/');

			return await Task.Run (() => {
				try {
					var clientDone = new ManualResetEvent (false);
					var reachable = false;
					var hostEntry = new DnsEndPoint (host, port);
					using (var socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {
						var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = hostEntry };
						socketEventArg.Completed += (s, e) => {
							reachable = e.SocketError == SocketError.Success;

							clientDone.Set ();
						};

						clientDone.Reset ();

						socket.ConnectAsync (socketEventArg);

						clientDone.WaitOne (msTimeout);

						return reachable;
					}
				} catch (Exception ex) {
					Debug.WriteLine ("Unable to reach: " + host + " Error: " + ex);
					return false;
				}
			});
		}

		/// <summary>
		/// Gets the list of all active connection types.
		/// </summary>
		public override IEnumerable<ConnectionType> ConnectionTypes {
			get {
				var status = Reachability.InternetConnectionStatus ();
				switch (status) {
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
		public override IEnumerable<UInt64> Bandwidths {
			get { return new UInt64[] { }; }
		}

		private bool disposed = false;

		/// <summary>
		/// Dispose
		/// </summary>
		/// <param name="disposing"></param>
		public override void Dispose (bool disposing)
		{
			if (!disposed) {
				if (disposing) {
					Reachability.ReachabilityChanged -= ReachabilityChanged;
					Reachability.Dispose ();
				}

				disposed = true;
			}

			base.Dispose (disposing);
		}
	}
}
