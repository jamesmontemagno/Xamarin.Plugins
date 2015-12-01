using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Battery.Abstractions;
using BatteryStatus = Android.OS.BatteryStatus;

namespace Plugin.Battery
{

  /// <summary>
  /// Broadcast receiver to get notifications from Android on battery change
  /// </summary>
  public class BatteryBroadcastReceiver : BroadcastReceiver
  {
    /// <summary>
    /// Action to call when battery level changes
    /// </summary>
    public static Action<BatteryChangedEventArgs> BatteryLevelChanged;

    /// <summary>
    /// Receiver BR
    /// </summary>
    /// <param name="context"></param>
    /// <param name="intent"></param>
    public override void OnReceive(Context context, Intent intent)
    {
      if (BatteryLevelChanged == null)
        return;

      var args = new BatteryChangedEventArgs();




      var level = intent.GetIntExtra(BatteryManager.ExtraLevel, -1);
      var scale = intent.GetIntExtra(BatteryManager.ExtraScale, -1);


      args.RemainingChargePercent = (int)Math.Floor(level * 100D / scale);

      if (intent.Action == Intent.ActionBatteryLow)
        args.IsLow = true;
      else if (intent.Action == Intent.ActionBatteryOkay)
        args.IsLow = false;
      else
        args.IsLow = args.RemainingChargePercent <= 15;

      // Are we charging / charged? works on phones, not emulators must check how.
      int status = intent.GetIntExtra(BatteryManager.ExtraStatus, -1);
      var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

      // How are we charging?
      var chargePlug = intent.GetIntExtra(BatteryManager.ExtraPlugged, -1);
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
      {
        args.PowerSource = PowerSource.Battery;
      }
      else if (usbCharge)
      {
        args.PowerSource = PowerSource.Usb;
      }
      else if (acCharge)
      {
        args.PowerSource = PowerSource.Ac;
      }
      else if (wirelessCharge)
      {
        args.PowerSource = PowerSource.Wireless;
      }
      else
      {
        args.PowerSource = PowerSource.Other;
      }


      if (isCharging)
        args.Status = Abstractions.BatteryStatus.Charging;
      else
      {
        switch (status)
        {
          case (int)BatteryStatus.Charging:
            args.Status = Abstractions.BatteryStatus.Charging;
            break;
          case (int)BatteryStatus.Discharging:
            args.Status = Abstractions.BatteryStatus.Discharging;
            break;
          case (int)BatteryStatus.Full:
            args.Status = Abstractions.BatteryStatus.Full;
            break;
          case (int)BatteryStatus.NotCharging:
            args.Status = Abstractions.BatteryStatus.NotCharging;
            break;
          default:
            args.Status = Abstractions.BatteryStatus.Unknown;
            break;
        }
      }
      BatteryLevelChanged(args);
    }
  }
}