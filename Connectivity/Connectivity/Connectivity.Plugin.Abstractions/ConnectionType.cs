using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Connectivity.Abstractions
{
    /// <summary>
    /// Type of connection
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Cellular connection, 3G, Edge, 4G, LTE
        /// </summary>
        Cellular,
        /// <summary>
        /// Wifi connection
        /// </summary>
        WiFi,
        /// <summary>
        /// Desktop or ethernet connection
        /// </summary>
        Desktop,
        /// <summary>
        /// Wimax (only android)
        /// </summary>
        Wimax,
        /// <summary>
        /// Other type of connection
        /// </summary>
        Other,
    }
}
