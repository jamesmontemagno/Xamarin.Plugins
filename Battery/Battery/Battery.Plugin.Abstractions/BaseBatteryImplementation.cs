using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Battery.Abstractions
{
  /// <summary>
  /// Base class for cross battery.
  /// </summary>
  public abstract class BaseBatteryImplementation : IBattery, IDisposable
  {
    /// <summary>
    /// Current battery level (0 - 100)
    /// </summary>
    public abstract int RemainingChargePercent
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
    public abstract PowerSource PowerSource
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

     /// <summary>
    /// Dispose of class and parent classes
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose up
    /// </summary>
    ~BaseBatteryImplementation()
    {
      Dispose(false); 
    }
    private bool disposed = false;
    /// <summary>
    /// Dispose method
    /// </summary>
    /// <param name="disposing"></param>
    public virtual void Dispose(bool disposing)
    {
      if(!disposed)
      {
        if(disposing)
        {
          //dispose only
        }

        disposed = true;
      }
    }

  }
}
