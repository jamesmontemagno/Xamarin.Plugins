using Battery.Plugin;
using Connectivity.Plugin;
using DeviceInfo.Plugin;
using ExternalMaps.Plugin;
using Geolocator.Plugin;
using ImageCircle.Forms.Plugin.Abstractions;
using Refractored.Xam.TTS;
using Refractored.Xam.TTS.Abstractions;
using Refractored.Xam.Vibrate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace TestAppForms
{
  public class App
  {
    public static Page GetMainPage()
    {

      return new NavigationPage(new Home())
            {
                BarBackgroundColor = Color.FromHex("2B84D3"),
                BarTextColor = Color.White
            };
    }
  }
}
