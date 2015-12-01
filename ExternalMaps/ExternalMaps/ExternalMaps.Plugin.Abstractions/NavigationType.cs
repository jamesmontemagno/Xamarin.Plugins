using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.ExternalMaps.Abstractions
{
  /// <summary>
  /// Type of navigation to initiate.
  /// </summary>
  public enum NavigationType
  {
    /// <summary>
    /// OS Default (usually driving)
    /// </summary>
    Default,
    /// <summary>
    /// Driving navigation
    /// </summary>
    Driving,
    /// <summary>
    /// Walking navigation
    /// </summary>
    Walking
  }
}
