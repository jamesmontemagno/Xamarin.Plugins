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

    private bool isConnected;
    private ConnectivityManager connectivityManager;
    public ConnectivityChangeBroadcastReceiver()
    {
        isConnected = IsConnected;
    }

    ConnectivityManager ConnectivityManager
    {
        get
        {
            connectivityManager = connectivityManager ??
                                   (ConnectivityManager)
                                   (Application.Context
                                       .GetSystemService(Context.ConnectivityService));
            return connectivityManager;
        }
    }

    /// <summary>
    /// Gets if there is an active internet connection
    /// </summary>
    bool IsConnected
    {
        get
        {
            try
            {
                var activeConnection = ConnectivityManager.ActiveNetworkInfo;

                return ((activeConnection != null) && activeConnection.IsConnected);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to get connected state - do you have ACCESS_NETWORK_STATE permission? - error: {0}", e);
                return false;
            }
        }
    }

    /// <summary>
    /// Received a notification via BR.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="intent"></param>
    public override void OnReceive(Context context, Intent intent)
    {
      if (intent.Action != ConnectivityManager.ConnectivityAction)
        return;

      var action = ConnectionChanged;
      if (action == null)
          return;

      var newConnection = IsConnected;
      if (newConnection == isConnected)
          return;

      isConnected = newConnection;

      action(new ConnectivityChangedEventArgs { IsConnected = isConnected });
    }
  }
}