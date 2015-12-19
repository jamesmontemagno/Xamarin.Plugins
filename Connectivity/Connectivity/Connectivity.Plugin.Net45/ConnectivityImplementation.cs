using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Connectivity
{
    /// <summary>
    /// Connectivity Implementation
    /// </summary>
    public class ConnectivityImplementation : BaseConnectivity
    {
        /// <summary>
        /// Bandwidths
        /// </summary>
        public override IEnumerable<ulong> Bandwidths => new List<ulong>();

        /// <summary>
        /// Connection types
        /// </summary>
        public override IEnumerable<ConnectionType> ConnectionTypes => new List<ConnectionType>();

        /// <summary>
        /// Is Connected
        /// </summary>
        public override bool IsConnected => true;

        /// <summary>
        /// Is Reachable
        /// </summary>
        /// <param name="host"></param>
        /// <param name="msTimeout"></param>
        /// <returns></returns>
        public override Task<bool> IsReachable(string host, int msTimeout = 5000) => Task.FromResult(true);

        /// <summary>
        /// IsReachable
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="msTimeout"></param>
        /// <returns></returns>
        public override Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000) => Task.FromResult(true);
    }
}
