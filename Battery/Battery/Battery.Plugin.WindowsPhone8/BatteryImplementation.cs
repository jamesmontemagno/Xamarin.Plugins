using Battery.Plugin.Abstractions;
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
      get { throw new NotImplementedException(); }
    }

    public override BatteryStatus Status
    {
      get { throw new NotImplementedException(); }
    }

    public override ChargeType ChargeType
    {
      get { throw new NotImplementedException(); }
    }
  }
}