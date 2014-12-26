#if __UNIFIED__
using Foundation;
#else
using MonoTouch.Foundation;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace Battery.Plugin
{
  public static class Helpers
  {
    public static NSObject Invoker;
    /// <summary>
    /// Ensures the invoked on main thread.
    /// </summary>
    /// <param name="action">Action to run on main thread.</param>
    public static void EnsureInvokedOnMainThread(Action action)
    {
      if (NSThread.Current.IsMainThread)
      {
        action();
        return;
      }
      if (Invoker == null)
        Invoker = new NSObject();

      Invoker.BeginInvokeOnMainThread(() => action());
    }



  }
}
