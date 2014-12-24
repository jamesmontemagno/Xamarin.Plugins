using Android.App;
using Android.Content;
using Android.OS;
using Battery.Plugin.Abstractions;
using System;
using Debug = System.Diagnostics.Debug;
using BatteryStatus = Android.OS.BatteryStatus;


namespace Battery.Plugin
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class BatteryImplementation : BaseCrossBattery
  {
    private BatteryBroadcastReceiver batteryReceiver;
    /// <summary>
    /// Default Constructor
    /// </summary>
    public BatteryImplementation()
    {
      try
      {
        batteryReceiver = new BatteryBroadcastReceiver();
        BatteryBroadcastReceiver.BatteryLevelChanged = OnBatteryChanged;
        Application.Context.RegisterReceiver(batteryReceiver, new IntentFilter(Intent.ActionBatteryChanged));
        Application.Context.RegisterReceiver(batteryReceiver, new IntentFilter(Intent.ActionBatteryLow));
        Application.Context.RegisterReceiver(batteryReceiver, new IntentFilter(Intent.ActionBatteryOkay));
      }
      catch
      {
        Debug.WriteLine("Unable to register for battery events, ensure you have android.permission.BATTERY_STATS set in AndroidManifest.");
        throw;
      }
    }
    /// <summary>
    /// Get the current battery level
    /// </summary>
    public override int Level
    {
      get 
      {
        try
        {
          using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
          {
            using (var battery = Application.Context.RegisterReceiver(null, filter))
            {
              var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
              var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

              return (int)Math.Floor(level * 100D / scale);
            }
          }
        }
        catch
        {
          Debug.WriteLine("Unable to gather battery level, ensure you have android.permission.BATTERY_STATS set in AndroidManifest.");
          throw;
        }

      }
    }

    public override Abstractions.BatteryStatus Status
    {
      get 
      {
        try
        {
          using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
          {
            using (var battery = Application.Context.RegisterReceiver(null, filter))
            {
              int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
              var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

              var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
              var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
              var acCharge = chargePlug == (int)BatteryPlugged.Ac;
              var wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

              isCharging = (usbCharge || acCharge || wirelessCharge);
              if (isCharging)
                return Abstractions.BatteryStatus.Charging;

              switch(status)
              {
                case (int)BatteryStatus.Charging:
                  return Abstractions.BatteryStatus.Charging;
                case (int)BatteryStatus.Discharging:
                  return Abstractions.BatteryStatus.Discharging;
                case (int)BatteryStatus.Full:
                  return Abstractions.BatteryStatus.Full;
                case (int)BatteryStatus.NotCharging:
                  return Abstractions.BatteryStatus.NotCharging;
                default:
                  return Abstractions.BatteryStatus.Unknown;
              }
            }
          }
        }
        catch
        {
          Debug.WriteLine("Unable to gather battery status, ensure you have android.permission.BATTERY_STATS set in AndroidManifest.");
          throw;
        }
      }
    }

    public override ChargeType ChargeType
    {
      get 
      {
        try
        {
          using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
          {
            using (var battery = Application.Context.RegisterReceiver(null, filter))
            {
              int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
              var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

              var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
              var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
              var acCharge = chargePlug == (int)BatteryPlugged.Ac;
              var wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

              isCharging = (usbCharge || acCharge || wirelessCharge);

              if (!isCharging)
                return Abstractions.ChargeType.None;
              else if (usbCharge)
                return Abstractions.ChargeType.Usb;
              else if (acCharge)
                return Abstractions.ChargeType.Ac;
              else if (wirelessCharge)
                return Abstractions.ChargeType.Wireless;
              else
                return Abstractions.ChargeType.Other;
            }
          }
        }
        catch
        {
          Debug.WriteLine("Unable to gather battery charge type, ensure you have android.permission.BATTERY_STATS set in AndroidManifest.");
          throw;
        }
      }
    }
  }
}