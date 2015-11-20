using Plugin.Battery.Abstractions;
#if __UNIFIED__
using UIKit;
using Foundation;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif
using System;


namespace Plugin.Battery
{
  /// <summary>
  /// Implementation for Battery
  /// </summary>
  public class BatteryImplementation : BaseBatteryImplementation
  {
    NSObject batteryLevel, batteryState;
    /// <summary>
    /// Default constructor
    /// </summary>
    public BatteryImplementation()
    {
      UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
      batteryLevel = UIDevice.Notifications.ObserveBatteryLevelDidChange(BatteryChangedNotification);
      batteryState = UIDevice.Notifications.ObserveBatteryStateDidChange(BatteryChangedNotification);
    }



    /// <summary>
    /// Battery changed notification triggered, bubble up.
    /// </summary>
    void BatteryChangedNotification(object sender, NSNotificationEventArgs args)
    {
      Helpers.EnsureInvokedOnMainThread(() =>
      {
        OnBatteryChanged(new BatteryChangedEventArgs
          {
            RemainingChargePercent = RemainingChargePercent,
            IsLow = RemainingChargePercent <= 15,
            Status = Status,
            PowerSource = PowerSource
          });
      });
    }


    /// <summary>
    /// Gets current level of battery
    /// </summary>
    public override int RemainingChargePercent
    {
      get 
      {
        return (int)(UIDevice.CurrentDevice.BatteryLevel * 100F);
      }
    }

    /// <summary>
    /// Gets current state of battery
    /// </summary>
    public override BatteryStatus Status
    {
      get 
      {
        switch (UIDevice.CurrentDevice.BatteryState)
        {
          case UIDeviceBatteryState.Charging:
            return BatteryStatus.Charging;
          case UIDeviceBatteryState.Full:
            return BatteryStatus.Full;
          case UIDeviceBatteryState.Unplugged:
            return BatteryStatus.Discharging;
          default:
            return BatteryStatus.Unknown;
        }
      }
    }

    /// <summary>
    /// Get charge type (guesstimate on iOS)
    /// </summary>
    public override PowerSource PowerSource
    {
      get 
      {
        switch (UIDevice.CurrentDevice.BatteryState)
        {
          case UIDeviceBatteryState.Charging:
            return Abstractions.PowerSource.Ac;
          case UIDeviceBatteryState.Full:
            return Abstractions.PowerSource.Ac;
          case UIDeviceBatteryState.Unplugged:
            return Abstractions.PowerSource.Battery;
          default:
            return Abstractions.PowerSource.Other;
        }
      }
    }

    private bool disposed = false;


    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    public override void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          UIDevice.CurrentDevice.BatteryMonitoringEnabled = false;
          if (batteryLevel != null)
          {
            batteryLevel.Dispose();
            batteryLevel = null;
          }

          if(batteryState != null)
          {
            batteryState.Dispose();
            batteryState = null;
          }
        }

        disposed = true;
      }

      base.Dispose(disposing);
    }
  }
}