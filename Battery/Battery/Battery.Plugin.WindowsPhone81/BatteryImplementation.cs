using Battery.Plugin.Abstractions;
using System;


namespace Battery.Plugin
{
  /// <summary>
  /// Implementation for Battery
  /// </summary>
  public class BatteryImplementation : BaseCrossBattery
  {
    public BatteryImplementation()
    {
      DefaultBattery.RemainingChargePercentChanged += (sender, args) =>
        {
          OnBatteryChanged(new BatteryChangedEventArgs
            {
              Level = DefaultBattery.RemainingChargePercent,
              IsLow = DefaultBattery.RemainingChargePercent <= 15,
              ChargeType = Abstractions.ChargeType.Other,
              Status = BatteryStatus.Unknown
            });
        }; 
    }
    Windows.Phone.Devices.Power.Battery battery;
    private Windows.Phone.Devices.Power.Battery DefaultBattery
    {
      get { return battery ?? (battery = Windows.Phone.Devices.Power.Battery.GetDefault()); }
    }
    public override int Level
    {
      get 
      {
        return DefaultBattery.RemainingChargePercent;
      }
    }

    public override BatteryStatus Status
    {
      get 
      {
        return BatteryStatus.Unknown;
      }
    }

    public override ChargeType ChargeType
    {
      get { return ChargeType.None; }
    }
  }
}