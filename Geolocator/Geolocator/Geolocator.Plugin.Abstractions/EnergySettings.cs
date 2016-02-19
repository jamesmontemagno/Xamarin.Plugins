using System;

namespace Plugin.Geolocator.Abstractions
{
	/// <summary>
	/// Energy settings for location listening (only applies to iOS).
	/// </summary>
	public class EnergySettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether the location manager should only listen for significant changes in location (approxmiately 500m), rather than continuous listening.
		/// </summary>
		/// <value><c>true</c> if location manager should listen for significant location updates; otherwise, <c>false</c> (default).</value>
		public bool ListenForSignificantChanges { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the location manager should defer location updates until an energy efficient time arrives, or distance and time criteria are met. Only available on iOS 6.0 and above.
		/// </summary>
		/// <value><c>true</c> to defer location updates; otherwise, <c>false</c> (default).</value>
		public bool DeferLocationUpdates { get; set; } = false;

		/// <summary>
		/// If deferring location updates, the minimum distance to travel before updates are delivered. Set to null for infinite wait.
		/// </summary>
		/// <value>The deferral distance meters (default:  500).</value>
		public double? DeferralDistanceMeters { get; set; } = 500;

		/// <summary>
		/// If deferring location updates, the minimum time that should elapse before updates are delivered. Set to null for infinite wait.
		/// </summary>
		/// <value>The time between updates (default:  5 minutes).</value>
		public TimeSpan? DeferralTime { get; set; } = TimeSpan.FromMinutes(5);
	}
}