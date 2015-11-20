using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Battery.Abstractions
{
  /// <summary>
  /// Currently how battery is being charged.
  /// </summary>
  public enum PowerSource
  {
    /// <summary>
    /// Not currently charging and on battery
    /// </summary>
    Battery,
    /// <summary>
    /// Wall charging
    /// </summary>
    Ac,
    /// <summary>
    /// USB charging
    /// </summary>
    Usb,
    /// <summary>
    /// Wireless charging
    /// </summary>
    Wireless,
    /// <summary>
    /// Other or unknown charging.
    /// </summary>
    Other
  }
}
