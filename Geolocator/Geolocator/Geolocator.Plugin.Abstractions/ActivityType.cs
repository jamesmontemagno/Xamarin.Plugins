using System;

namespace Plugin.Geolocator.Abstractions
{
    /// <summary>
	/// Activity type (iOS only). Used to determine when to automatically pause location updates. 
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// GPS is being used for an unknown activity.
        /// </summary>
        Other,

        /// <summary>
        /// GPS is being used specifically during vehicular navigation to track location changes to the automobile. This activity might cause location updates to be paused only when the vehicle does not move for an extended period of time.
        /// </summary>
        AutomotiveNavigation,

        /// <summary>
        /// GPS is being used to track any pedestrian-related activity. This activity might cause location updates to be paused only when the user does not move a significant distance over a period of time.
        /// </summary>
        Fitness,

        /// <summary>
        /// GPS is being used to track movements for other types of vehicular navigation that are not automobile related. For example, you would use this to track navigation by boat, train, or plane. Do not use this type for pedestrian navigation tracking. This activity might cause location updates to be paused only when the vehicle does not move a significant distance over a period of time.
        /// </summary>
        OtherNavigation
    }
}