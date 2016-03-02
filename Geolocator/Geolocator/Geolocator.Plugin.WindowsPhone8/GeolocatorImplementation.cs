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

using System;
using System.Device.Location;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;


namespace Plugin.Geolocator
{
    /// <summary>
    /// Implementation for Geolocator
    /// </summary>
    public class GeolocatorImplementation : IGeolocator
    {
        public GeolocatorImplementation()
        {
            DesiredAccuracy = 100;
        }
        public event EventHandler<PositionErrorEventArgs> PositionError;
        public event EventHandler<PositionEventArgs> PositionChanged;

        /// <inheritdoc/>
        public bool AllowsBackgroundUpdates
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public bool PausesLocationUpdatesAutomatically
        { get; set; }

        /// <inheritdoc/>
        public ActivityType ActivityType
        { get; set; }

        /// <inheritdoc/>
        public bool IsGeolocationAvailable
        {
            get { return true; }
        }
        /// <inheritdoc/>
        public bool IsGeolocationEnabled
        {
            get
            {
                if (this.watcher != null)
                    this.isEnabled = (this.watcher.Permission == GeoPositionPermission.Granted && this.watcher.Status != GeoPositionStatus.Disabled);
                else
                    this.isEnabled = GetEnabled();

                return this.isEnabled;
            }
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
            get { return true; }
        }
        /// <inheritdoc/>
        public bool IsListening
        {
            get { return (this.watcher != null); }
        }

        /// <inheritdoc/>
        public Task<Position> GetPositionAsync(int timeoutMilliseconds = Timeout.Infinite, CancellationToken? cancelToken = null, bool includeHeading = false)
        {
            if (!cancelToken.HasValue)
                cancelToken = CancellationToken.None;

            if (timeoutMilliseconds <= 0 && timeoutMilliseconds != Timeout.Infinite)
                throw new ArgumentOutOfRangeException("timeoutMilliseconds", "timeout must be greater than or equal to 0");

            return new SinglePositionListener(DesiredAccuracy, timeoutMilliseconds, cancelToken.Value).Task;
        }
        /// <inheritdoc/>
        public Task<bool> StartListeningAsync(int minTime, double minDistance, bool includeHeading = false, ListenerEnergySettings energySettings = null)
        {
            if (minTime < 0)
                throw new ArgumentOutOfRangeException("minTime");
            if (minDistance < 0)
                throw new ArgumentOutOfRangeException("minDistance");
            if (IsListening)
                throw new InvalidOperationException("This Geolocator is already listening");

            this.watcher = new GeoCoordinateWatcher(GetAccuracy(DesiredAccuracy));
            this.watcher.MovementThreshold = minDistance;
            this.watcher.PositionChanged += WatcherOnPositionChanged;
            this.watcher.StatusChanged += WatcherOnStatusChanged;
            this.watcher.Start();

            return Task.FromResult(true);
        }
        /// <inheritdoc/>
        public Task<bool> StopListeningAsync()
        {
            if (this.watcher == null)
                return Task.FromResult(true);

            this.watcher.PositionChanged -= WatcherOnPositionChanged;
            this.watcher.StatusChanged -= WatcherOnStatusChanged;
            this.watcher.Stop();
            this.watcher.Dispose();
            this.watcher = null;

            return Task.FromResult(true);
        }

        private GeoCoordinateWatcher watcher;
        private bool isEnabled;

        private static bool GetEnabled()
        {
            GeoCoordinateWatcher w = new GeoCoordinateWatcher();
            try
            {
                w.Start(true);
                bool enabled = (w.Permission == GeoPositionPermission.Granted && w.Status != GeoPositionStatus.Disabled);
                w.Stop();

                return enabled;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                w.Dispose();
            }
        }

        private async void WatcherOnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            this.isEnabled = (this.watcher.Permission == GeoPositionPermission.Granted && this.watcher.Status != GeoPositionStatus.Disabled);

            GeolocationError error;
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    error = GeolocationError.Unauthorized;
                    break;

                case GeoPositionStatus.NoData:
                    error = GeolocationError.PositionUnavailable;
                    break;

                default:
                    return;
            }

            await StopListeningAsync();

            var perror = PositionError;
            if (perror != null)
                perror(this, new PositionErrorEventArgs(error));
        }

        private void WatcherOnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Position p = GetPosition(e.Position);
            if (p != null)
            {
                var pupdate = PositionChanged;
                if (pupdate != null)
                    pupdate(this, new PositionEventArgs(p));
            }
        }

        internal static GeoPositionAccuracy GetAccuracy(double desiredAccuracy)
        {
            if (desiredAccuracy < 100)
                return GeoPositionAccuracy.High;

            return GeoPositionAccuracy.Default;
        }

        internal static Position GetPosition(GeoPosition<GeoCoordinate> position)
        {
            if (position.Location.IsUnknown)
                return null;

            var p = new Position();
            p.Accuracy = position.Location.HorizontalAccuracy;
            p.Longitude = position.Location.Longitude;
            p.Latitude = position.Location.Latitude;

            if (!Double.IsNaN(position.Location.VerticalAccuracy) && !Double.IsNaN(position.Location.Altitude))
            {
                p.AltitudeAccuracy = position.Location.VerticalAccuracy;
                p.Altitude = position.Location.Altitude;
            }

            if (!Double.IsNaN(position.Location.Course))
                p.Heading = position.Location.Course;

            if (!Double.IsNaN(position.Location.Speed))
                p.Speed = position.Location.Speed;

            p.Timestamp = position.Timestamp.ToUniversalTime();

            return p;
        }
    }

}