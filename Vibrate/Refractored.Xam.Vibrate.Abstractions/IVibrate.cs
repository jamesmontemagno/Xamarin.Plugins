using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refractored.Xam.Vibrate.Abstractions
{
  /// <summary>
  /// Vibration interface
  /// </summary>
    public interface IVibrate
    {
      /// <summary>
      /// Vibrate the phone for specified amount of time
      /// </summary>
      /// <param name="milliseconds">Time in Milliseconds to vibrate. 500ms is default</param>
      void Vibration(int milliseconds = 500);
    }
}
