using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.Geolocator.Abstractions
{
    /// <summary>
    /// Interface for Geolocator
    /// </summary>
    public interface IGeolocator
    {
        /// <summary>
        /// Position error event handler
        /// </summary>
        event EventHandler<PositionErrorEventArgs> PositionError;
        /// <summary>
        /// Position changed event handler
        /// </summary>
        event EventHandler<PositionEventArgs> PositionChanged;

        /// <summary>
        /// Desired accuracy in meteres
        /// </summary>
        double DesiredAccuracy { get; set; }
        /// <summary>
        /// Gets if you are listening for location changes
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Gets if device supports heading
        /// </summary>
        bool SupportsHeading { get; }

        /// <summary>
        /// Gets or sets if background updates should be allowed on the geolocator.
        /// </summary>
        bool AllowsBackgroundUpdates { get; set; }

        /// <summary>
        /// Gets or sets if the location updates should be paused automatically (iOS). Default:  false
        /// </summary>
        bool PausesLocationUpdatesAutomatically { get; set; }

		/// <summary>
		/// Gets or sets the activity type that should be used to determine when to automatically pause location updates (iOS). Default:  ActivityType.Other
		/// </summary>
		/// <value>Activity type.</value>
		ActivityType ActivityType { get; set; }

        /// <summary>
        /// Gets if geolocation is available on device
        /// </summary>
        bool IsGeolocationAvailable { get; }

        /// <summary>
        /// Gets if geolocation is enabled on device
        /// </summary>
        bool IsGeolocationEnabled { get; }

        /// <summary>
        /// Gets position async with specified parameters
        /// </summary>
        /// <param name="timeoutMilliseconds">Timeout in milliseconds to wait, Default Infinite</param>
        /// <param name="token">Cancelation token</param>
        /// <param name="includeHeading">If you would like to include heading</param>
        /// <returns>Position</returns>
        Task<Position> GetPositionAsync(int timeoutMilliseconds = Timeout.Infinite, CancellationToken? token = null, bool includeHeading = false);

        /// <summary>
        /// Start lisenting for changes
        /// </summary>
        /// <param name="minTime">Time</param>
        /// <param name="minDistance">Distance</param>
        /// <param name="includeHeading">Include heading or not</param>
		/// <param name="energySettings">Optional energy settings (iOS only)</param>
		Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false, ListenerEnergySettings energySettings = null);

        /// <summary>
        /// Stop linstening
        /// </summary>
        Task<bool> StopListeningAsync();

    }
}
