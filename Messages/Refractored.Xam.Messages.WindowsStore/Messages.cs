
using Windows.UI.Popups;

namespace Refractored.Xam.Messages
{
  public class Messages
  {

    public Messages()
    {

    }

    public void ShowMessage(string text)
    {
      var dialog = new MessageDialog(text);
      dialog.ShowAsync();
    }
  }
}
