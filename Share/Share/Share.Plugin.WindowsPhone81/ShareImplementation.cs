using Share.Plugin.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

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
                await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to open browser task: " + ex.Message);
            }
        }

        string text, title;
        DataTransferManager dataTransferManager;
        public async Task Share(string text, string title = null)
        {
            this.text = text;
            this.title = title;
            if (dataTransferManager == null)
            {
                dataTransferManager = DataTransferManager.GetForCurrentView();
                dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareTextHandler);
            }
            DataTransferManager.ShowShareUI();
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            // The Title is mandatory
            request.Data.Properties.Title = title ?? string.Empty;
            request.Data.Properties.Description = text;
            request.Data.SetText(title ?? string.Empty);
        }
    }
}