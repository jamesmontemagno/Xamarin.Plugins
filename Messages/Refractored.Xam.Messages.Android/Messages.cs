
using Android.App;
using Android.Widget;

namespace Refractored.Xam.Messages
{
  public class Messages
  {
    public Messages()
    {

    }

    public void ShowMessage(string text)
    {
      Toast.MakeText(Application.Context, text, ToastLength.Long).Show();
    }
  }
}
