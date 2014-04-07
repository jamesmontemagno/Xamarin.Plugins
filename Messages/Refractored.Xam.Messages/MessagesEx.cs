using System;
#if __ANDROID__
using Android.App;
#elif __IOS__
using MonoTouch.UIKit;
#elif WINDOWS_PHONE
using System.Windows;
#elif NETFX_CORE
using Windows.UI.Popups;
#endif


namespace Refractored.Xam.Messages
{
  public class MessagesEx
  {
    public void ShowMessage(string title, string message)
    {
#if __ANDROID__
      new AlertDialog.Builder(Application.Context)
        .SetTitle(title)
        .SetMessage(message)
        .SetPositiveButton("OK", delegate { })
        .Show();
#elif __IOS__
      var uiAlert = new UIAlertView(title, message, null, "OK");
      uiAlert.Show();
#elif WINDOWS_PHONE
       MessageBox.Show(title, message, MessageBoxButton.OK);
#elif NETFX_CORE
      var dialog = new MessageDialog(title, message);
      dialog.ShowAsync();
#else
      NotImplementedInReferenceAssembly(); 
#endif
    }


    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Xam.Plugins.Settings NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
