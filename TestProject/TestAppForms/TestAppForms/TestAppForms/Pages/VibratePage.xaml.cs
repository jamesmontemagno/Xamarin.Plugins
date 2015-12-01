using Plugin.Vibrate;
using System;
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
