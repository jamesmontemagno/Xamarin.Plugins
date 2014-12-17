using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connectivity.Plugin.Abstractions
{
  public abstract class BaseConnectivity : IConnectivity
  {
    public abstract bool IsConnected
    {
      get;
    }

    public abstract Task<bool> IsReachable(string host, int msTimeout = 5000);

    public abstract Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000);

    public abstract IEnumerable<ConnectionType> ConnectionTypes
    {
      get;
    }

    public abstract IEnumerable<ulong> Bandwidths
    {
      get;
    }

    protected virtual void OnConnectivityChanged(ConnectivityChangedEventArgs e)
    {
      if (ConnectivityChanged == null)
        return;

      ConnectivityChanged(this, e);
    }

    public event ConnectivityChangedEventHandler ConnectivityChanged;
  }
}
