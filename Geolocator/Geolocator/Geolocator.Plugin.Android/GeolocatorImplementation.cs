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
using Plugin.Geolocator.Abstractions;
using System;
using System.Threading.Tasks;
using Android.Locations;
using System.Threading;
using Android.App;
using Android.OS;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Plugin.Permissions;

namespace Plugin.Geolocator
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class GeolocatorImplementation : IGeolocator
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GeolocatorImplementation()
        {
            DesiredAccuracy = 100;
            manager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
            providers = manager.GetProviders(enabledOnly: false).Where(s => s != LocationManager.PassiveProvider).ToArray();
        }
        /// <inheritdoc/>
        public event EventHandler<PositionErrorEventArgs> PositionError;
        /// <inheritdoc/>
        public event EventHandler<PositionEventArgs> PositionChanged;
        /// <inheritdoc/>
        public bool IsListening
        {
            get { return listener != null; }
        }
        /// <inheritdoc/>
        public double DesiredAccuracy
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public bool AllowsBackgroundUpdates
        {
            get;
            set;
        }
        /// <inheritdoc/>
        public bool PausesLocationUpdatesAutomatically
        {
            get;
            set;
        }

		/// <inheritdoc/>
		public ActivityType ActivityType
		{
			get;
			set;
		}

        /// <inheritdoc/>
        public bool SupportsHeading
        {
            get
            {
                return true; //Kind of, you should use the  Compass plugin for better results
            }
        }
        /// <inheritdoc/>
        public bool IsGeolocationAvailable
        {
            get { return providers.Length > 0; }
        }
        /// <inheritdoc/>
        public bool IsGeolocationEnabled
        {
            get { return providers.Any(manager.IsProviderEnabled); }
        }


        /// <inheritdoc/>
        public async Task<Position> GetPositionAsync(int timeoutMilliseconds = Timeout.Infinite, CancellationToken? cancelToken = null, bool includeHeading = false)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permissions.Abstractions.Permission.Location).ConfigureAwait(false);
            if (status != Permissions.Abstractions.PermissionStatus.Granted)
            {
                Console.WriteLine("Currently does not have Location permissions, requesting permissions");

                var request = await CrossPermissions.Current.RequestPermissionsAsync(Permissions.Abstractions.Permission.Location);

                if (request[Permissions.Abstractions.Permission.Location] != Permissions.Abstractions.PermissionStatus.Granted)
                {
                    Console.WriteLine("Location permission denied, can not get positions async.");
                    return null;
                }
                providers = manager.GetProviders(enabledOnly: false).Where(s => s != LocationManager.PassiveProvider).ToArray();
            }

            if (providers.Length == 0)
            {
                providers = manager.GetProviders(enabledOnly: false).Where(s => s != LocationManager.PassiveProvider).ToArray();
            }


            if (timeoutMilliseconds <= 0 && timeoutMilliseconds != Timeout.Infinite)
                throw new ArgumentOutOfRangeException("timeoutMilliseconds", "timeout must be greater than or equal to 0");

            if (!cancelToken.HasValue)
                cancelToken = CancellationToken.None;


            var tcs = new TaskCompletionSource<Position>();

            if (!IsListening)
            {
                GeolocationSingleListener singleListener = null;
                singleListener = new GeolocationSingleListener((float)DesiredAccuracy, timeoutMilliseconds, providers.Where(manager.IsProviderEnabled),
                    finishedCallback: () =>
                {
                    for (int i = 0; i < providers.Length; ++i)
                        manager.RemoveUpdates(singleListener);
                });

                if (cancelToken != CancellationToken.None)
                {
                    cancelToken.Value.Register(() =>
                    {
                        singleListener.Cancel();

                        for (int i = 0; i < providers.Length; ++i)
                            manager.RemoveUpdates(singleListener);
                    }, true);
                }

                try
                {
                    Looper looper = Looper.MyLooper() ?? Looper.MainLooper;

                    int enabled = 0;
                    for (int i = 0; i < providers.Length; ++i)
                    {
                        if (manager.IsProviderEnabled(providers[i]))
                            enabled++;

                        manager.RequestLocationUpdates(providers[i], 0, 0, singleListener, looper);
                    }

                    if (enabled == 0)
                    {
                        for (int i = 0; i < providers.Length; ++i)
                            manager.RemoveUpdates(singleListener);

                        tcs.SetException(new GeolocationException(GeolocationError.PositionUnavailable));
                        return await tcs.Task.ConfigureAwait(false);
                    }
                }
                catch (Java.Lang.SecurityException ex)
                {
                    tcs.SetException(new GeolocationException(GeolocationError.Unauthorized, ex));
                    return await tcs.Task.ConfigureAwait(false);
                }

                return await singleListener.Task.ConfigureAwait(false);
            }

            // If we're already listening, just use the current listener
            lock (positionSync)
            {
                if (lastPosition == null)
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
                    tcs.SetResult(lastPosition);
                }
            }

            return await tcs.Task.ConfigureAwait(false);
        }

        /// <inheritdoc/>
		public async Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false, ListenerEnergySettings energySettings = null)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permissions.Abstractions.Permission.Location).ConfigureAwait(false);
            if (status != Permissions.Abstractions.PermissionStatus.Granted)
            {
                Console.WriteLine("Currently does not have Location permissions, requesting permissions");

                var request = await CrossPermissions.Current.RequestPermissionsAsync(Permissions.Abstractions.Permission.Location);

                if (request[Permissions.Abstractions.Permission.Location] != Permissions.Abstractions.PermissionStatus.Granted)
                {
                    Console.WriteLine("Location permission denied, can not get positions async.");
                    return false;
                }
                providers = manager.GetProviders(enabledOnly: false).Where(s => s != LocationManager.PassiveProvider).ToArray();
            }

            if (providers.Length == 0)
            {
                providers = manager.GetProviders(enabledOnly: false).Where(s => s != LocationManager.PassiveProvider).ToArray();
            }

            if (minTime < 0)
                throw new ArgumentOutOfRangeException("minTime");
            if (minDistance < 0)
                throw new ArgumentOutOfRangeException("minDistance");
            if (IsListening)
                throw new InvalidOperationException("This Geolocator is already listening");

            listener = new GeolocationContinuousListener(manager, TimeSpan.FromMilliseconds(minTime), providers);
            listener.PositionChanged += OnListenerPositionChanged;
            listener.PositionError += OnListenerPositionError;

            Looper looper = Looper.MyLooper() ?? Looper.MainLooper;
            for (int i = 0; i < providers.Length; ++i)
                manager.RequestLocationUpdates(providers[i], minTime, (float)minDistance, listener, looper);

            return true;
        }
        /// <inheritdoc/>
        public Task<bool> StopListeningAsync()
        {
            if (listener == null)
                return Task.FromResult(true);

            listener.PositionChanged -= OnListenerPositionChanged;
            listener.PositionError -= OnListenerPositionError;

            for (int i = 0; i < providers.Length; ++i)
                manager.RemoveUpdates(listener);

            listener = null;
            return Task.FromResult(true);
        }

        private string[] providers;
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

            lock (positionSync)
            {
                lastPosition = e.Position;

                var changed = PositionChanged;
                if (changed != null)
                    changed(this, e);
            }
        }
        /// <inheritdoc/>
        private async void OnListenerPositionError(object sender, PositionErrorEventArgs e)
        {
            await StopListeningAsync();

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