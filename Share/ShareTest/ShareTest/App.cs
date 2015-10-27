using Share.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ShareTest
{
    public class App : Application
    {
        public App()
        {
            var button = new Button
            {
                Text = "Share some text"
            };

            var button2 = new Button
            {
                Text = "Open browser"
            };

            button.Clicked += (sender, args) =>
            {
                CrossShare.Current.Share("Follow @JamesMontemagno on Twitter", "Share");
            };

            button2.Clicked += (sender, args) =>
            {
                CrossShare.Current.OpenBrowser("http://motzcod.es");
            };

            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            XAlign = TextAlignment.Center,
                            Text = "Welcome to Share Plugin Sample!"
                        }, button, button2

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
