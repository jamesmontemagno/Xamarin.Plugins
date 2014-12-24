using Battery.Plugin.Abstractions;
#if __UNIFIED__
using UIKit;
using Foundation;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif
using System;


namespace Battery.Plugin
{
  /// <summary>
  /// Implementation for Battery
  /// </summary>
  public class BatteryImplementation : BaseCrossBattery
  {

    /// <summary>
    /// Default constructor
    /// </summary>
    public BatteryImplementation()
    {
      UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
      UIDevice.Notifications.ObserveBatteryLevelDidChange(delegate { ObserveBatteryChanged(); });
      UIDevice.Notifications.ObserveBatteryStateDidChange(delegate { ObserveBatteryChanged(); });
    }

    /// <summary>
    /// Battery changed notification triggered, bubble up.
    /// </summary>
    /// <param name="args"></param>
    void ObserveBatteryChanged()
    {
      OnBatteryChanged(new BatteryChangedEventArgs
        {
          Level = Level,
          IsLow = Level <= 15,
          Status = Status,
          ChargeType = ChargeType
        });
    }


    /// <summary>
    /// Gets current level of battery
    /// </summary>
    public override int Level
    {
      get 
      {
        return (int)UIDevice.CurrentDevice.BatteryLevel;
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
    public override ChargeType ChargeType
    {
      get 
      {
        switch (UIDevice.CurrentDevice.BatteryState)
        {
          case UIDeviceBatteryState.Charging:
            return Abstractions.ChargeType.Ac;
          case UIDeviceBatteryState.Full:
            return Abstractions.ChargeType.Ac;
          case UIDeviceBatteryState.Unplugged:
            return Abstractions.ChargeType.None;
          default:
            return Abstractions.ChargeType.Other;
        }
      }
    }

  }
}