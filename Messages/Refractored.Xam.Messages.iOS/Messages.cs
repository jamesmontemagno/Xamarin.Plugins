using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace Refractored.Xam.Messages
{
  public class Messages
  {
    public Messages()
    {

    }

    public void ShowMessage(string text)
    {
      var uiAlert = new UIAlertView(string.Empty, text, null, "OK");
      uiAlert.Show();
    }
  }
}
