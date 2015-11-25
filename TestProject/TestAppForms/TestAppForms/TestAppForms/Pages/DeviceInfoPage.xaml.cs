using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class DeviceInfoPage : ContentPage
  {
    public DeviceInfoPage()
    {
      InitializeComponent();
      Content = new StackLayout
      {
        Padding = 10,
        Spacing = 10,
        Children = 
        {
          new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId()},
          new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true)},
          new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello")},
          new Label{ Text = "Generated AppId: " + CrossDeviceInfo.Current.GenerateAppId(true, "hello", "world")},
          new Label{ Text = "Id: " + CrossDeviceInfo.Current.Id},
          new Label{ Text = "Model: " + CrossDeviceInfo.Current.Model},
          new Label{ Text = "Platform: " + CrossDeviceInfo.Current.Platform},
          new Label{ Text = "Version: " + CrossDeviceInfo.Current.Version},  
        }
      };
    }
  }
}
