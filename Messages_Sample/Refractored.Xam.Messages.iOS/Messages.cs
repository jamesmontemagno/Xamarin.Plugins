
using UIKit;

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
