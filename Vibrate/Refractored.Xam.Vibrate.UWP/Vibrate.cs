using Plugin.Vibrate.Abstractions;
using System;
using Windows.Foundation.Metadata;

namespace Plugin.Vibrate
{
    public class Vibrate : IVibrate
    {
        public void Vibration(int milliseconds = 500)
        {
            if (ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice"))
            {
                var v = Windows.Phone.Devices.Notification.VibrationDevice.GetDefault();

                if (milliseconds < 0)
                    milliseconds = 0;
                else if (milliseconds > 5000)
                    milliseconds = 5000;

                var time = TimeSpan.FromMilliseconds(milliseconds);
                v.Vibrate(time);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Vibration not supported on thid device family.");
            }
        }
    }
}
