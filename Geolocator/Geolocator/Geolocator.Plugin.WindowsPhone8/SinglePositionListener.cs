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
using System.Device.Location;
using System.Threading;
using System.Threading.Tasks;


namespace Plugin.Geolocator
{
  internal class SinglePositionListener
  {
    internal SinglePositionListener(double accuracy, int timeout, CancellationToken cancelToken)
    {
      cancelToken.Register(HandleTimeout, true);
      this.desiredAccuracy = accuracy;
      this.start = DateTime.Now;
      this.timeout = timeout;

      System.Threading.Tasks.Task.Factory.StartNew(() =>
      {
        this.watcher = new GeoCoordinateWatcher(GeolocatorImplementation.GetAccuracy(accuracy));
        this.watcher.PositionChanged += WatcherOnPositionChanged;
        this.watcher.StatusChanged += WatcherOnStatusChanged;

        this.watcher.Start();
      });

      if (timeout != Timeout.Infinite)
        this.timer = new Timer(HandleTimeout, null, timeout, Timeout.Infinite);

      Task.ContinueWith(Cleanup);
    }

    public Task<Position> Task
    {
      get { return this.tcs.Task; }
    }

    private GeoPosition<GeoCoordinate> bestPosition;
    private GeoCoordinateWatcher watcher;
    private readonly double desiredAccuracy;
    private readonly DateTimeOffset start;
    private readonly Timer timer;
    private readonly int timeout;
    private readonly TaskCompletionSource<Position> tcs = new TaskCompletionSource<Position>();

    private void Cleanup(Task task)
    {
      this.watcher.PositionChanged -= WatcherOnPositionChanged;
      this.watcher.StatusChanged -= WatcherOnStatusChanged;

      this.watcher.Stop();
      this.watcher.Dispose();

      if (this.timer != null)
        this.timer.Dispose();
    }

    private void HandleTimeout(object state)
    {
      if (state != null && (bool)state)
        this.tcs.TrySetCanceled();

      if (this.bestPosition != null)
        this.tcs.TrySetResult(GeolocatorImplementation.GetPosition(this.bestPosition));
      else
        this.tcs.TrySetCanceled();
    }

    private void WatcherOnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
    {
      switch (e.Status)
      {
        case GeoPositionStatus.NoData:
          this.tcs.TrySetException(new GeolocationException(GeolocationError.PositionUnavailable));
          break;

        case GeoPositionStatus.Disabled:
          this.tcs.TrySetException(new GeolocationException(GeolocationError.Unauthorized));
          break;
      }
    }

    private void WatcherOnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
    {
      if (e.Position.Location.IsUnknown)
        return;

      bool isRecent = timeout == Timeout.Infinite || (e.Position.Timestamp - this.start).TotalMilliseconds < this.timeout;

      if (e.Position.Location.HorizontalAccuracy <= this.desiredAccuracy && isRecent)
        this.tcs.TrySetResult(GeolocatorImplementation.GetPosition(e.Position));

      if (this.bestPosition == null || e.Position.Location.HorizontalAccuracy < this.bestPosition.Location.HorizontalAccuracy)
        this.bestPosition = e.Position;
    }
  }
}
