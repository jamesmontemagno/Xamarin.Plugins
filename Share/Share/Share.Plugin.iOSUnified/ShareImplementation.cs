using Foundation;
using Share.Plugin.Abstractions;
using System;
using System.Threading.Tasks;
using UIKit;

namespace Share.Plugin
{
    /// <summary>
    /// Implementation for Share
    /// </summary>
    public class ShareImplementation : IShare
    {
        public static async Task Init()
        {
            var test = DateTime.UtcNow;
        }
        public async Task OpenBrowser(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException(nameof(url), "Url can not be null or empty");
            try
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser: " + ex.Message);
            }

        }

        public async Task Share(string text, string title = null)
        {
            try
            {
                var items = new NSObject[] { new NSString(text) };
                var activityController = new UIActivityViewController(items, null);
                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView =
                      UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers != null
                        ? UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers[0].View
                        : UIApplication.SharedApplication.KeyWindow.RootViewController.View;
                }
                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share text" + ex.Message);
            }
        }

        public Task ShareUrl(string url, string title = null, string subject = null)
        {
            throw new NotImplementedException();
        }
    }
}