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
using Android.Net;
using Connectivity.Plugin.Abstractions;

namespace Connectivity.Plugin
{
  /// <summary>
  /// Broadcast receiver to get notifications from Android on connectivity change
  /// </summary>
  [BroadcastReceiver(Enabled=true, Label="Connectivity Plugin Broadcast Receiver")]
  //[IntentFilter(new[] { "android.net.conn.CONNECTIVITY_CHANGE" })]
  public class ConnectivityChangeBroadcastReceiver : BroadcastReceiver
  {
    /// <summary>
    /// Action to call when connetivity changes
    /// </summary>
    public static Action<ConnectivityChangedEventArgs> ConnectionChanged;
    
    /// <summary>
    /// Received a notification via BR.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="intent"></param>
    public override void OnReceive(Context context, Intent intent)
    {
      if (intent.Action != ConnectivityManager.ConnectivityAction)
        return;

      var noConnectivity = intent.GetBooleanExtra(ConnectivityManager.ExtraNoConnectivity, false);

      if (ConnectionChanged == null)
        return;

      ConnectionChanged(new ConnectivityChangedEventArgs { IsConnected = !noConnectivity });
    }
  }
}