using DeviceInfo.Plugin;
using Refractored.Xam.TTS;
using Refractored.Xam.TTS.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
    public partial class TextToSpeechPage : ContentPage
    {

     

        static CrossLocale? locale = null;

        public TextToSpeechPage()
        {
            InitializeComponent();


            var defaultRate = 1.0f;
            if(Device.OS == TargetPlatform.iOS)
            {
                if (CrossDeviceInfo.Current.Version.StartsWith("8."))
                    defaultRate = .125f;
                else if (CrossDeviceInfo.Current.Version.StartsWith("7."))
                    defaultRate = .25f;
                else
                    defaultRate = .5f;

            }
            sliderRate.Value = defaultRate;
            speakButton.Clicked += async (sender, args) =>
            {
                try
                {
                    //CrossSms.Current.SendSms("Hello there!", "+16024926689");
                    var text = "The quick brown fox jumped over the lazy dog.";
                    if (useDefaults.IsToggled)
                    {
                        CrossTextToSpeech.Current.Speak(text);
                        return;
                    }

                    CrossTextToSpeech.Current.Speak(text,
                        pitch: (float)sliderPitch.Value,
                        speakRate: (float)sliderRate.Value,
                        volume: (float)sliderVolume.Value,
                        crossLocale: locale);
                }
                catch (Exception ex)
                {
                    Xamarin.Insights.Report(ex);
                    await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
                }
            };

            languageButton.Clicked += async (sender, args) =>
            {
                try
                {
                    var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
                    var items = locales.Select(a => a.ToString()).ToArray();

                    if (Device.OS == TargetPlatform.Android)
                    {
                        DependencyService.Get<IDialogs>().DisplayActionSheet("Language", "OK",
                            items,
                            which =>
                            {
                                languageButton.Text = items[which];
                                locale = locales.ElementAt(which);
                            });
                    }
                    else
                    {
                        var selected = await DisplayActionSheet("Language", "OK", null, items);
                        if (string.IsNullOrWhiteSpace(selected) || selected == "OK")
                            return;
                        languageButton.Text = selected;
                        locale = new CrossLocale { Language = selected };//fine for iOS/WP
                    }
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
