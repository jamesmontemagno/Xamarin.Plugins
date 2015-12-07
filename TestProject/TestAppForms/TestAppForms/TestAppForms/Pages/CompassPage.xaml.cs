using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Compass.Abstractions;
using System.Diagnostics;
using Plugin.Compass;
using Xamarin.Forms.Xaml;

namespace TestAppForms.Pages
{
    public partial class CompassPage : ContentPage
    {
        ICompass compass;

        public CompassPage()
        {
            InitializeComponent();
            compass = CrossCompass.Current;

            label.VerticalOptions = LayoutOptions.CenterAndExpand;
            label.HorizontalOptions = LayoutOptions.CenterAndExpand;

            buttonStart.Clicked += (sender, args) =>
            {
                compass.CompassChanged += Compass_CompassChanged;
                compass.Start();

                buttonStart.IsEnabled = false;
                buttonStop.IsEnabled = true;

            };

            buttonStop.Clicked += (sender, args) =>
            {
                if (compass != null)
                {
                    compass.Stop();

                    Device.BeginInvokeOnMainThread(() =>
                        {
                            buttonStart.IsEnabled = true;
                            buttonStop.IsEnabled = false;
                        });
                }

            };

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (compass != null)
            {
                compass.Stop();

                compass.CompassChanged -= Compass_CompassChanged;

            }
        }

        void Compass_CompassChanged(object sender, CompassChangedEventArgs e)
        {
            Debug.WriteLine("*** Compass Heading = {0}", e.Heading);

            label.Text = $"Heading = {e.Heading}";
        }
    }
}

