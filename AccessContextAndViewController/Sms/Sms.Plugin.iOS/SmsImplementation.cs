#if __UNIFIED__
using MessageUI;
using UIKit;
#else
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
#endif
using Sms.Plugin.Abstractions;
using System;


namespace Sms.Plugin
{
  /// <summary>
  /// Implementation for Sms
  /// </summary>
  public class SmsImplementation : ISms
  {
    private MFMessageComposeViewController smsController;
    public void SendSms(string body, string phoneNumber)
    {
      if (!MFMailComposeViewController.CanSendMail)
        return;

      smsController = new MFMessageComposeViewController();

      smsController.Recipients = new[] { phoneNumber };
      smsController.Body = body;

      EventHandler<MFMessageComposeResultEventArgs> handler = null;
      handler = (sender, args) =>
      {
        smsController.Finished -= handler;

        var uiViewController = sender as UIViewController;
        if (uiViewController == null)
        {
          throw new ArgumentException("sender");
        }

        uiViewController.DismissViewControllerAsync(true);
      };

      smsController.Finished += handler;

      UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(smsController, true);
    }
  }
}