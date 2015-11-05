using System;

using Xamarin.Forms;
using Geolocator.Plugin;

namespace GeolocatorTests
{
    public class App : Application
    {
        public App()
        {

            var label = new Label
            {
                Text = "Click Get Location"
            };

            var button = new Button
            {
                    Text = "Get Location"
            };

            button.Clicked += async (sender, e) => 
                {
                    try
                    {
                        button.IsEnabled = false;
                        label.Text = "Getting...";
                        var test = await CrossGeolocator.Current.GetPositionAsync(10000);
                        label.Text = "Lat: " + test.Latitude.ToString() + " Long: " + test.Longitude.ToString();
                    }
                    catch(Exception ex)
                    {
                        label.Text = ex.Message;
                    }
                    finally
                    {
                        button.IsEnabled = true;
                    }
                };
            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                                    label, button
                    }
                }
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

