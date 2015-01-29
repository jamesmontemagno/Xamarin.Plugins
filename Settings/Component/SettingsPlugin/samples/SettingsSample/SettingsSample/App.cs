using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SettingsSample
{
  public class App : Application
  {
    public App()
    {
      var editField = new Entry
      {
        Placeholder = "Enter text for setting",
        Text = SettingsSample.Helpers.Settings.GeneralSettings
      };
      var buttonSave = new Button
      {
        Text = "Save Setting"
      };

      var buttonGet = new Button
      {
        Text = "Get Setting"
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
							Text = "Enter value to save to settings:"
						}, editField,
            buttonSave,
            buttonGet
					}
        }
      };

      buttonSave.Clicked += (sender, args) =>
        {
          SettingsSample.Helpers.Settings.GeneralSettings = editField.Text;
        };

      buttonGet.Clicked += (sender, args) =>
        {
          MainPage.DisplayAlert("Current Value:", SettingsSample.Helpers.Settings.GeneralSettings, "OK");
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
