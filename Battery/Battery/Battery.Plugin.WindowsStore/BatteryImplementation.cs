using Plugin.Battery.Abstractions;
using System;
using System.Diagnostics;


namespace Plugin.Battery
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
      get 
      {
        Debug.WriteLine("Not supported on Windows Store, will return 100");
        return 100; 
      }
    }

    /// <summary>
    /// Not supported in Window Store, always returns full
    /// </summary>
    public override BatteryStatus Status
    {
      get 
      {
        Debug.WriteLine("Not supported on Windows Store, will return Full");
        return BatteryStatus.Full; 
      }
    }

    /// <summary>
    /// No supported in Windows Store, always returns AC
    /// </summary>
    public override PowerSource PowerSource
    {
      get
      {
        Debug.WriteLine("Not supported on Windows Store, will return Ac.");
        return Abstractions.PowerSource.Ac; 
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
        }

        disposed = true;
      }

      base.Dispose(disposing);
    }
  }
}