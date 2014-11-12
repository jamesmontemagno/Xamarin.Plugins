
using System.Windows;

namespace Refractored.Xam.Messages
{
  public class Messages
  {
    public Messages()
    {

    }

    public void ShowMessage(string text)
    {
      MessageBox.Show(string.Empty, text, MessageBoxButton.OK);
    }
  }
}
