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
            vibrateButton.Clicked += async (sender, args) =>
            {
                try
                {
                    CrossVibrate.Current.Vibration((int)sliderVibrate.Value);

                }
                catch (Exception ex)
                {
                    Xamarin.Insights.Report(ex);
                    await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
                }
            };
        }
    }
}
