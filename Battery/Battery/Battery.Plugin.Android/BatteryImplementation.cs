using Android.App;
using Android.Content;
using Android.OS;
using Plugin.Battery.Abstractions;
using System;
using Debug = System.Diagnostics.Debug;
using BatteryStatus = Android.OS.BatteryStatus;


namespace Plugin.Battery
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class BatteryImplementation : BaseBatteryImplementation
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
    public override int RemainingChargePercent
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

    /// <summary>
    /// Get Current battery status
    /// </summary>
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
              bool wirelessCharge = false;
#if __ANDROID_17__
              if ((int)Build.VERSION.SdkInt >= 17)
              {
                  wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;
              }
#endif

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

    /// <summary>
    /// Get current power source of device
    /// </summary>
    public override PowerSource PowerSource
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

              bool wirelessCharge = false;
#if __ANDROID_17__
        if ((int)Build.VERSION.SdkInt >= 17)
        {
            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;
        }
#endif
              
              isCharging = (usbCharge || acCharge || wirelessCharge);

              if (!isCharging)
                return Abstractions.PowerSource.Battery;
              else if (usbCharge)
                return Abstractions.PowerSource.Usb;
              else if (acCharge)
                return Abstractions.PowerSource.Ac;
              else if (wirelessCharge)
                return Abstractions.PowerSource.Wireless;
              else
                return Abstractions.PowerSource.Other;
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

    private bool disposed = false;


    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    public override void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          if(batteryReceiver != null)
          {
            Application.Context.UnregisterReceiver(batteryReceiver);
            batteryReceiver = null;
          }
        }

        disposed = true;
      }

      base.Dispose(disposing);
    }
  }
}