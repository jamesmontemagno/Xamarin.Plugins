using System;

namespace Plugin.Battery.Abstractions
{
  /// <summary>
  /// Interface for Battery
  /// </summary>
  public interface IBattery : IDisposable
  {
    /// <summary>
    /// Current battery level 0 - 100
    /// </summary>
    int RemainingChargePercent { get; }

    /// <summary>
    /// Current status of the battery
    /// </summary>
    BatteryStatus Status { get; }

    /// <summary>
    /// Currently how the battery is being charged.
    /// </summary>
    PowerSource PowerSource { get; }

    /// <summary>
    /// Event handler when battery changes
    /// </summary>
    event BatteryChangedEventHandler BatteryChanged;

  }

  /// <summary>
  /// Arguments to pass to event handlers
  /// </summary>
  public class BatteryChangedEventArgs : EventArgs
  {
    /// <summary>
    /// If the battery level is considered low
    /// </summary>
    public bool IsLow { get; set; }
    /// <summary>
    /// Gets if there is an active internet connection
    /// </summary>
    public int RemainingChargePercent { get; set; }

    /// <summary>
    /// Current status of battery
    /// </summary>
    public BatteryStatus Status { get; set; }

    /// <summary>
    /// Get the source of power.
    /// </summary>
    public PowerSource PowerSource { get; set; }
  }

  /// <summary>
  /// Battery Level changed event handlers
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void BatteryChangedEventHandler(object sender, BatteryChangedEventArgs e);
  
}
