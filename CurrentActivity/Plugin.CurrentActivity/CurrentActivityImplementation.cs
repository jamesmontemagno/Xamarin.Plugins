using Android.App;
using Plugin.CurrentActivity.Abstractions;
using System;


namespace Plugin.CurrentActivity
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class CurrentActivityImplementation : ICurrentActivity
    {
        public Activity Activity
        {
            get;
            set;
        }
    }
}