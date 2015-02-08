using ExternalMaps.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class ExternalMapsPage : ContentPage
  {
    public ExternalMapsPage()
    {
      InitializeComponent();
      navigateLatLong.Clicked += (sender, args) =>
      {
        CrossExternalMaps.Current.NavigateTo("Space Needle", 47.6204, -122.3491);
      };

      navigateAddress.Clicked += (sender, args) =>
      {
        CrossExternalMaps.Current.NavigateTo("Xamarin", "394 pacific ave.", "San Francisco", "CA", "94111", "USA", "USA");
      };

    }
  }
}
