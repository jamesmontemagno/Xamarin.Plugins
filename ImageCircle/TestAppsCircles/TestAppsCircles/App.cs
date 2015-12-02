using ImageCircle.Forms.Plugin.Abstractions;
using System;

using Xamarin.Forms;

namespace TestAppsCircles
{
    public class App : Application
    {
        public App()
        {

            CircleImage pink = null;
            var button = new Button
            {
                Text = "Change Colors"
            };

            button.Clicked += (sender, args) =>
            {
                if (pink.BorderColor == Color.Pink)
                {
                    pink.BorderThickness = 20;
                    pink.BorderColor = Color.Maroon;
                    pink.FillColor = Color.Pink;
                }
                else
                {
                    pink.BorderThickness = 3;
                    pink.BorderColor = Color.Pink;
                    pink.FillColor = Color.Olive;
                }
            };
            var stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
                Padding = 20,
                Children =
                {
                    button,
                      (pink = new CircleImage
                      {
                        BorderColor = Color.Pink,
                        FillColor = Color.Olive,
                        BorderThickness = 3,
                        HeightRequest = 150,
                        WidthRequest = 150,
                        Aspect = Aspect.AspectFill,
                        HorizontalOptions = LayoutOptions.Center,
                        //Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/55/Tamarin_portrait.JPG"))
                      }),
                      new CircleImage
                      {
                        BorderColor = Color.Purple,
                        FillColor = Color.Transparent,
                        BorderThickness = 6,
                        HeightRequest = 150,
                        WidthRequest = 150,
                        Aspect = Aspect.AspectFill,
                        HorizontalOptions = LayoutOptions.Center,
                        //Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/e/ed/Saguinus_tripartitus_-_Golden-mantled_Tamarin.jpg"))
                      },
                      new CircleImage
                      {
                        BorderColor = Color.Yellow,
                        FillColor = Color.Yellow,
                        BorderThickness = 9,
                        HeightRequest = 150,
                        WidthRequest = 150,
                        Aspect = Aspect.AspectFill,
                        HorizontalOptions = LayoutOptions.Center,
                        Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/53/Golden_Lion_Tamarin_Leontopithecus_rosalia.jpg"))
                      },
                }
            };

            // The root page of your application
            MainPage = new ContentPage
            {
                Content = stack
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
