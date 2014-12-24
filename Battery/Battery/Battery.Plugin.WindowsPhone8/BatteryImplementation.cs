using Battery.Plugin.Abstractions;
using Microsoft.Phone.Info;
using System;


namespace Battery.Plugin
{
  /// <summary>
  /// Implementation for Battery
  /// </summary>
  public class BatteryImplementation : BaseCrossBattery
  {
    public override int Level
    {
      get 
      {
        throw new NotImplementedException();
      }
    }

    public override BatteryStatus Status
    {
      get 
      {
        switch (DeviceStatus.PowerSource)
        {
          case PowerSource.Battery:
            return BatteryStatus.Discharging;
          default:
            return BatteryStatus.Charging;
        }
      }
    }

    public override ChargeType ChargeType
    {
      get 
      {
        switch (DeviceStatus.PowerSource)
        {
          case PowerSource.Battery:
            return ChargeType.None;
          default:
            return ChargeType.Ac;
        }
      }
    }
  }
}