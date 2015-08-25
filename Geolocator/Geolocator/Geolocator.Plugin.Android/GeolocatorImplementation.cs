//
//  Copyright 2011-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using Geolocator.Plugin.Abstractions;
using System;
using System.Threading.Tasks;
using Android.Locations;
using System.Threading;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using System.Linq;
using Android.Content;
using Android.Content.PM;


namespace Geolocator.Plugin
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class GeolocatorImplementation : IGeolocator
    {
        public GeolocatorImplementation()
        {
            DesiredAccuracy = 50;
            this.manager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            this.providers = manager.GetProviders(enabledOnly: false).Where(s => s != LocationManager.PassiveProvider).ToArray();
        }
        /// <inheritdoc/>
        public event EventHandler<PositionErrorEventArgs> PositionError;
        /// <inheritdoc/>
        public event EventHandler<PositionEventArgs> PositionChanged;
        /// <inheritdoc/>
        public bool IsListening
        {
            get { return this.listener != null; }
        }
        /// <inheritdoc/>
        public double DesiredAccuracy
        {
            get;
            set;
        }
        /// <inheritdoc/>
        public bool SupportsHeading
        {
            get
            {
                return false;
                //				if (this.headingProvider == null || !this.manager.IsProviderEnabled (this.headingProvider))
                //				{
                //					Criteria c = new Criteria { BearingRequired = true };
                //					string providerName = this.manager.GetBestProvider (c, enabledOnly: false);
                //
                //					LocationProvider provider = this.manager.GetProvider (providerName);
                //
                //					if (provider.SupportsBearing())
                //					{
                //						this.headingProvider = providerName;
                //						return true;
                //					}
                //					else
                //					{
                //						this.headingProvider = null;
                //						return false;
                //					}
                //				}
                //				else
                //					return true;
            }
        }
        /// <inheritdoc/>
        public bool IsGeolocationAvailable
        {
            get { return this.providers.Length > 0; }
        }
        /// <inheritdoc/>
        public bool IsGeolocationEnabled
        {
            get { return this.providers.Any(this.manager.IsProviderEnabled); }
        }

        private bool CheckPermission(string permission)
        {
            var res = Android.App.Application.Context.CheckCallingOrSelfPermission(permission);
            return (res == Permission.Granted);
        }
        /// <inheritdoc/>
        public Task<Position> GetPositionAsync(int timeoutMilliseconds = Timeout.Infinite, CancellationToken? cancelToken = null, bool includeHeading = false)
        {

            if (!CheckPermission("android.permission.ACCESS_COARSE_LOCATION"))
            {
                Console.WriteLine("Unable to get location, ACCESS_COARSE_LOCATION not set.");
                return null;
            }


            if (!CheckPermission("android.permission.ACCESS_FINE_LOCATION"))
            {
                Console.WriteLine("Unable to get location, ACCESS_FINE_LOCATION not set.");
                return null;
            }

            if (timeoutMilliseconds <= 0 && timeoutMilliseconds != Timeout.Infinite)
                throw new ArgumentOutOfRangeException("timeoutMilliseconds", "timeout must be greater than or equal to 0");

            if (!cancelToken.HasValue)
                cancelToken = CancellationToken.None;


            var tcs = new TaskCompletionSource<Position>();

            if (!IsListening)
            {
                GeolocationSingleListener singleListener = null;
                singleListener = new GeolocationSingleListener((float)DesiredAccuracy, timeoutMilliseconds, this.providers.Where(this.manager.IsProviderEnabled),
                    finishedCallback: () =>
                {
                    for (int i = 0; i < this.providers.Length; ++i)
                        this.manager.RemoveUpdates(singleListener);
                });

                if (cancelToken != CancellationToken.None)
                {
                    cancelToken.Value.Register(() =>
                    {
                        singleListener.Cancel();

                        for (int i = 0; i < this.providers.Length; ++i)
                            this.manager.RemoveUpdates(singleListener);
                    }, true);
                }

                try
                {
                    Looper looper = Looper.MyLooper() ?? Looper.MainLooper;

                    int enabled = 0;
                    for (int i = 0; i < this.providers.Length; ++i)
                    {
                        if (this.manager.IsProviderEnabled(this.providers[i]))
                            enabled++;

                        this.manager.RequestLocationUpdates(this.providers[i], 0, 0, singleListener, looper);
                    }

                    if (enabled == 0)
                    {
                        for (int i = 0; i < this.providers.Length; ++i)
                            this.manager.RemoveUpdates(singleListener);

                        tcs.SetException(new GeolocationException(GeolocationError.PositionUnavailable));
                        return tcs.Task;
                    }
                }
                catch (Java.Lang.SecurityException ex)
                {
                    tcs.SetException(new GeolocationException(GeolocationError.Unauthorized, ex));
                    return tcs.Task;
                }

                return singleListener.Task;
            }

            // If we're already listening, just use the current listener
            lock (this.positionSync)
            {
                if (this.lastPosition == null)
                {
                    if (cancelToken != CancellationToken.None)
                    {
                        cancelToken.Value.Register(() => tcs.TrySetCanceled());
                    }

                    EventHandler<PositionEventArgs> gotPosition = null;
                    gotPosition = (s, e) =>
                    {
                        tcs.TrySetResult(e.Position);
                        PositionChanged -= gotPosition;
                    };

                    PositionChanged += gotPosition;
                }
                else
                {
                    tcs.SetResult(this.lastPosition);
                }
            }

            return tcs.Task;
        }

        /// <inheritdoc/>
        public void StartListening(int minTime, double minDistance, bool includeHeading = false)
        {
            if (minTime < 0)
                throw new ArgumentOutOfRangeException("minTime");
            if (minDistance < 0)
                throw new ArgumentOutOfRangeException("minDistance");
            if (IsListening)
                throw new InvalidOperationException("This Geolocator is already listening");

            this.listener = new GeolocationContinuousListener(this.manager, TimeSpan.FromMilliseconds(minTime), this.providers);
            this.listener.PositionChanged += OnListenerPositionChanged;
            this.listener.PositionError += OnListenerPositionError;

            Looper looper = Looper.MyLooper() ?? Looper.MainLooper;
            for (int i = 0; i < this.providers.Length; ++i)
                this.manager.RequestLocationUpdates(providers[i], minTime, (float)minDistance, listener, looper);
        }
        /// <inheritdoc/>
        public void StopListening()
        {
            if (this.listener == null)
                return;

            this.listener.PositionChanged -= OnListenerPositionChanged;
            this.listener.PositionError -= OnListenerPositionError;

            for (int i = 0; i < this.providers.Length; ++i)
                this.manager.RemoveUpdates(this.listener);

            this.listener = null;
        }

        private readonly string[] providers;
        private readonly LocationManager manager;
        private string headingProvider;

        private GeolocationContinuousListener listener;

        private readonly object positionSync = new object();
        private Position lastPosition;
        /// <inheritdoc/>
        private void OnListenerPositionChanged(object sender, PositionEventArgs e)
        {
            if (!IsListening) // ignore anything that might come in afterwards
                return;

            lock (this.positionSync)
            {
                this.lastPosition = e.Position;

                var changed = PositionChanged;
                if (changed != null)
                    changed(this, e);
            }
        }
        /// <inheritdoc/>
        private void OnListenerPositionError(object sender, PositionErrorEventArgs e)
        {
            StopListening();

            var error = PositionError;
            if (error != null)
                error(this, e);
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal static DateTimeOffset GetTimestamp(Location location)
        {
            return new DateTimeOffset(Epoch.AddMilliseconds(location.Time));
        }
    }
}