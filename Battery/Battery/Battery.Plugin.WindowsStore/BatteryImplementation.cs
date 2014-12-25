using Battery.Plugin.Abstractions;
using System;


namespace Battery.Plugin
{
  /// <summary>
  /// Implementation for Battery
  /// </summary>
  public class BatteryImplementation : BaseBatteryImplementation
  {
    /// <summary>
    /// Not supported in Windows Store, always returns 100
    /// </summary>
    public override int RemainingChargePercent
    {
      get { return 100; }
    }

    /// <summary>
    /// Not supported in Window Store, always returns full
    /// </summary>
    public override BatteryStatus Status
    {
      get { return BatteryStatus.Full; }
    }

    /// <summary>
    /// No supported in Windows Store, always returns AC
    /// </summary>
    public override PowerSource PowerSource
    {
      get { return Abstractions.PowerSource.Ac; }
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
        }

        disposed = true;
      }

      base.Dispose(disposing);
    }
  }
}