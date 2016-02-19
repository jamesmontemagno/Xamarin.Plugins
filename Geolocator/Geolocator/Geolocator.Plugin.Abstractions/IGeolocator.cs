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
        /// Gets or sets if the location updates should be paused automatically (iOS)
        /// </summary>
        bool PausesLocationUpdatesAutomatically { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Plugin.Geolocator.Abstractions.IGeolocator"/> defers
		/// location updates (iOS).
		/// </summary>
		/// <value><c>true</c> if defer location updates; otherwise, <c>false</c>.</value>
		bool DefersLocationUpdates { get; set; }

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
		/// <param name="defersLocationUpdates">(Only on iOS) Whether or not location updates are deferred.</param>
		/// <param name="deferralDistanceMeters">(Only on iOS) The distance (in meters) from the current location that must be travelled before event delivery resumes. To specify an unlimited distance, pass -1.</param>
		/// <param name="deferralTimeSeconds">(Only on iOS) The amount of time (in seconds) from the current time that must pass before event delivery resumes. To specify an unlimited amount of time, pass -1.</param>
		Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false, bool defersLocationUpdates = false, double deferralDistanceMeters = -1, double deferralTimeSeconds = -1);

        /// <summary>
        /// Stop linstening
        /// </summary>
        Task<bool> StopListeningAsync();

    }
}
