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
  [BroadcastReceiver(Enabled=true, Label="Connectivity Plugin Broadcast Receiver")]
  [IntentFilter(new[] { "android.net.conn.CONNECTIVITY_CHANGE" })]
  public class ConnectivityChangeBroadcastReceiver : BroadcastReceiver
  {
    public static Action<ConnectivityChangedEventArgs> ConnectionChanged;
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