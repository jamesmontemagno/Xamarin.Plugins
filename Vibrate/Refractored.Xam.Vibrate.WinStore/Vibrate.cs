using Refractored.Xam.Vibrate.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refractored.Xam.Vibrate
{
  /// <summary>
  /// Vibration implemenation on Windows Store
  /// </summary>
  public class Vibrate : IVibrate
  {
    /// <summary>
    /// Vibration (no effect windows store)
    /// </summary>
    /// <param name="milliseconds">milliseconds to vibrate for</param>
    public void Vibration(int milliseconds = 500)
    {
      Debug.WriteLine("Vibration not supported on Windows Store apps.");
    }
  }
}
