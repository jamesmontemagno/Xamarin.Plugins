using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestAppForms.Pages
{
  public partial class ImageCirclePage : ContentPage
  {
    public ImageCirclePage()
    {
      InitializeComponent();

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
				}
				else
				{
					pink.BorderThickness = 3;
					pink.BorderColor = Color.Pink;
				}
			};
      Content = new StackLayout
      {
        VerticalOptions = LayoutOptions.Center,
        Spacing = 10,
        Padding = 10,
        Children =
        {
			button,
              (pink = new CircleImage
              {
                BorderColor = Color.Pink,
                BorderThickness = 3,
                HeightRequest = 150,
                WidthRequest = 150,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/55/Tamarin_portrait.JPG"))
              }),
              new CircleImage
              {
                BorderColor = Color.Purple,
                BorderThickness = 6,
                HeightRequest = 150,
                WidthRequest = 150,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/e/ed/Saguinus_tripartitus_-_Golden-mantled_Tamarin.jpg"))
              },
              new CircleImage
              {
                BorderColor = Color.Yellow,
                BorderThickness = 9,
                HeightRequest = 150,
                WidthRequest = 150,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/53/Golden_Lion_Tamarin_Leontopithecus_rosalia.jpg"))
              },
        }
      };
    }
  }
}
