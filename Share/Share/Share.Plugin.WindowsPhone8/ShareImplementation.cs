using Microsoft.Phone.Tasks;
using Share.Plugin.Abstractions;
using System;
using System.Threading.Tasks;

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
                throw new ArgumentException(nameof(url), "Url can not be null or empty.");
            try
            {
                var webBrowserTask = new WebBrowserTask();

                webBrowserTask.Uri = new Uri(url, UriKind.Absolute);

                webBrowserTask.Show();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open browser task: " + ex.Message);
            }
        }

        public async Task Share(string text, string title = null)
        {
            var task = new ShareStatusTask
            {
                Status = text
            };

            task.Show();
        }
    }
}