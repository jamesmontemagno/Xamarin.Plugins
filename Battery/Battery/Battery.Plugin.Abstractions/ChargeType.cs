using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battery.Plugin.Abstractions
{
  /// <summary>
  /// Currently how battery is being charged.
  /// </summary>
  public enum ChargeType
  {
    /// <summary>
    /// Not currently charging
    /// </summary>
    None,
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
