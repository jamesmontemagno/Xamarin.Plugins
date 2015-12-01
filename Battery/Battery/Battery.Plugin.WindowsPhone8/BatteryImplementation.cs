using Plugin.Battery.Abstractions;
using Microsoft.Phone.Info;
using System;
using System.Windows;
using System.Windows.Threading;


namespace Plugin.Battery
{
  /// <summary>
  /// Implementation for Battery
  /// </summary>
  public class BatteryImplementation : BaseBatteryImplementation
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public BatteryImplementation()
    {
      DefaultBattery.RemainingChargePercentChanged += RemainingChargePercentChanged;
    }

    void RemainingChargePercentChanged(object sender, object e)
    {

      Deployment.Current.Dispatcher.BeginInvoke(() =>
      {
        OnBatteryChanged(new BatteryChangedEventArgs
        {
          RemainingChargePercent = DefaultBattery.RemainingChargePercent,
          IsLow = DefaultBattery.RemainingChargePercent <= 15,
          PowerSource = this.PowerSource,
          Status = this.Status
        }); 
      });
      
    }

    Windows.Phone.Devices.Power.Battery battery;
    private Windows.Phone.Devices.Power.Battery DefaultBattery
    {
      get { return battery ?? (battery = Windows.Phone.Devices.Power.Battery.GetDefault()); }
    }
    /// <summary>
    /// Get remaining charge percent
    /// </summary>
    public override int RemainingChargePercent
    {
      get 
      {
        return DefaultBattery.RemainingChargePercent;
      }
    }

    /// <summary>
    /// Get current battery status
    /// </summary>
    public override BatteryStatus Status
    {
      get 
      {
        if (DefaultBattery.RemainingChargePercent == 100)
          return BatteryStatus.Full;

        switch (DeviceStatus.PowerSource)
        {
          case Microsoft.Phone.Info.PowerSource.Battery:
            return BatteryStatus.Discharging;
          default:
            return BatteryStatus.Charging;
        }
      }
    }

    /// <summary>
    /// Get current charge type, either 
    /// </summary>
    public override Abstractions.PowerSource PowerSource
    {
      get 
      {
        switch (DeviceStatus.PowerSource)
        {
          case Microsoft.Phone.Info.PowerSource.Battery:
            return Abstractions.PowerSource.Battery;
          default:
            return Abstractions.PowerSource.Ac;
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
          DefaultBattery.RemainingChargePercentChanged -= RemainingChargePercentChanged;
        }

        disposed = true;
      }

      base.Dispose(disposing);
    }
  }
}