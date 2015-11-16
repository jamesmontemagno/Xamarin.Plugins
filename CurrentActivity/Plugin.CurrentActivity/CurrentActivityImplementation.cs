using Android.App;
using System;


namespace Plugin.CurrentActivity
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class CurrentActivityImplementation : ICurrentActivity
    {
        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        /// <value>The activity.</value>
        public Activity Activity
        {
            get;
            set;
        }
    }
}