using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Plugin.Geolocator
{
    /// <summary>
    /// Geolocator implementation
    /// </summary>
    public class GeolocatorImplementation : IGeolocator
    {
        /// <summary>
        /// Allows background updates
        /// </summary>
        public bool AllowsBackgroundUpdates { get; set; } = false;

        /// <summary>
        /// Accuracy
        /// </summary>
        public double DesiredAccuracy { get; set; } = 100;

        /// <summary>
        /// Is Available
        /// </summary>
        public bool IsGeolocationAvailable => false;

        /// <summary>
        /// Is Enabled
        /// </summary>
        public bool IsGeolocationEnabled => false;

        /// <summary>
        /// Is Listening
        /// </summary>
        public bool IsListening => false;

        /// <summary>
        /// Pauses
        /// </summary>
        public bool PausesLocationUpdatesAutomatically { get; set; }

        /// <summary>
        /// Supports heading
        /// </summary>
        public bool SupportsHeading => false;

        /// <summary>
        /// Position changed
        /// </summary>
        public event EventHandler<PositionEventArgs> PositionChanged;
        /// <summary>
        /// Position error
        /// </summary>
        public event EventHandler<PositionErrorEventArgs> PositionError;

        /// <summary>
        /// Get position
        /// </summary>
        /// <param name="timeoutMilliseconds"></param>
        /// <param name="token"></param>
        /// <param name="includeHeading"></param>
        /// <returns></returns>
        public Task<Position> GetPositionAsync(int timeoutMilliseconds = -1, CancellationToken? token = default(CancellationToken?), bool includeHeading = false)
            => Task.FromResult<Position>(null);

        /// <summary>
        /// Start listenting
        /// </summary>
        /// <param name="minTime"></param>
        /// <param name="minDistance"></param>
        /// <param name="includeHeading"></param>
        /// <returns></returns>
        public Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false)
            => Task.FromResult(false);

        /// <summary>
        /// Stop listening
        /// </summary>
        /// <returns></returns>
        public Task<bool> StopListeningAsync()
            => Task.FromResult(false);
    }
}
