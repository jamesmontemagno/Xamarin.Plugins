using Android.Content;
using Share.Plugin.Abstractions;
using System;
using System.Threading.Tasks;

namespace Share.Plugin
{
  /// <summary>
  /// Implementation for Feature
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
            {
                throw new ArgumentException(nameof(url), "Url can not be null or empty");
            }
            try
            {
                var intent = new Intent(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse(url));

                intent.SetFlags(ActivityFlags.ClearTop);
                intent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser: " + ex.Message);
            }
        }

        public async Task Share(string text, string title = null)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, text);

            intent.SetFlags(ActivityFlags.ClearTop);
            intent.SetFlags(ActivityFlags.NewTask);
            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(chooserIntent);
        }

        public async Task ShareUrl(string url, string title = null, string subject = null)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, url);
            intent.PutExtra(Intent.ExtraSubject, subject ?? string.Empty);

            intent.SetFlags(ActivityFlags.ClearTop);
            intent.SetFlags(ActivityFlags.NewTask);
            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            Android.App.Application.Context.StartActivity(chooserIntent);

        }

    }
}