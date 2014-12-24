using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battery.Plugin.Abstractions
{
  /// <summary>
  /// Base class for cross battery.
  /// </summary>
  public abstract class BaseCrossBattery : IBattery
  {
    /// <summary>
    /// Current battery level (0 - 100)
    /// </summary>
    public abstract int Level
    {
      get;
    }

    /// <summary>
    /// Current status of battery
    /// </summary>
    public abstract BatteryStatus Status
    {
      get;
    }

    /// <summary>
    /// Current charge type of battery
    /// </summary>
    public abstract ChargeType ChargeType
    {
      get;
    }



    /// <summary>
    /// When battery level changes
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnBatteryChanged(BatteryChangedEventArgs e)
    {
      if (BatteryChanged == null)
        return;

      BatteryChanged(this, e);
    }


  
    /// <summary>
    /// Event that fires when Battery status, level, power changes
    /// </summary>
    public event BatteryChangedEventHandler BatteryChanged;

  }
}
