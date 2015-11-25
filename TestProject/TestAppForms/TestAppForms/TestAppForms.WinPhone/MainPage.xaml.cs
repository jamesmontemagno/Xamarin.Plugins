using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;
//using ImageCircle.Forms.Plugin.WindowsPhone;
//using Refractored.Xam.Forms.Vibrate.WinPhone;


namespace TestAppForms.WinPhone
{
  public partial class MainPage : PhoneApplicationPage
  {
    public MainPage()
    {
      InitializeComponent();

      Forms.Init();
      Xamarin.Insights.Initialize(Helpers.Settings.InsightsKey);
      Xamarin.Insights.ForceDataTransmission = true;
      //Vibrate.Init();
      //ImageCircleRenderer.Init();
      Content = TestAppForms.App.GetMainPage().ConvertPageToUIElement(this);
    }
  }
}
