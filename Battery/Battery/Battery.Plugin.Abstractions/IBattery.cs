using System;

namespace Battery.Plugin.Abstractions
{
  /// <summary>
  /// Interface for Battery
  /// </summary>
  public interface IBattery
  {
    /// <summary>
    /// Current battery level 0 - 100
    /// </summary>
    int Level { get; }

    /// <summary>
    /// Current status of the battery
    /// </summary>
    BatteryStatus Status { get; }

    /// <summary>
    /// Currenlty how the battery is being charged.
    /// </summary>
    ChargeType ChargeType { get; }

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
    public int Level { get; set; }

    /// <summary>
    /// Current status of battery
    /// </summary>
    public BatteryStatus Status { get; set; }

    public ChargeType ChargeType { get; set; }
  }

  /// <summary>
  /// Battery Level changed event handlers
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void BatteryChangedEventHandler(object sender, BatteryChangedEventArgs e);
  
}
