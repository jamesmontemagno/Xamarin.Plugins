using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Battery.Abstractions
{
  /// <summary>
  /// Current status of battery
  /// </summary>
  public enum BatteryStatus
  {
    /// <summary>
    /// Plugged in and charging
    /// </summary>
    Charging,
    /// <summary>
    /// Battery is being drained currently
    /// </summary>
    Discharging,
    /// <summary>
    /// Battery is full completely
    /// </summary>
    Full,
    /// <summary>
    /// Not charging, but not discharging either
    /// </summary>
    NotCharging,
    /// <summary>
    /// Unknown or other status
    /// </summary>
    Unknown

  }
}
