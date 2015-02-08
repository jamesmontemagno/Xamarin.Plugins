using Refractored.Xam.Vibrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class VibratePage : ContentPage
  {
    public VibratePage()
    {
      InitializeComponent();
      vibrateButton.Clicked += (sender, args) =>
      {
        CrossVibrate.Current.Vibration((int)sliderVibrate.Value);
      };
    }
  }
}
