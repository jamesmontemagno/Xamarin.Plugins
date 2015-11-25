using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppForms.Helpers;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class SettingsPage : ContentPage
  {
    public SettingsPage()
    {
      InitializeComponent();
      var editField = new Entry
      {
        Placeholder = "Enter text for setting",
        Text = Helpers.Settings.GeneralSettings
      };
      var buttonSave = new Button
      {
        Text = "Save Setting"
      };

      var buttonGet = new Button
      {
        Text = "Get Setting"
      };

      Content = new StackLayout
      {
        Padding =10,
        Spacing = 10,
        VerticalOptions = LayoutOptions.Center,
        Children = {
						new Label {
							VerticalTextAlignment = TextAlignment.Center,
							Text = "Enter value to save to settings:"
						}, editField,
            buttonSave,
            buttonGet
					}
      };

      buttonSave.Clicked += (sender, args) =>
      {
        Settings.GeneralSettings = editField.Text;
      };

      buttonGet.Clicked += async (sender, args) =>
      {
        await DisplayAlert("Current Value:", Helpers.Settings.GeneralSettings, "OK");
      };
    }
  }
}
