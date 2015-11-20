using Plugin.Vibrate.Abstractions;
using System.Diagnostics;

namespace Plugin.Vibrate
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
