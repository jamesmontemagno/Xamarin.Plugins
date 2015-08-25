using ImageCircle.Forms.Plugin.WindowsStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;

[assembly: ExportRenderer(typeof(ImageCircle.Forms.Plugin.Abstractions.CircleImage), typeof(ImageCircleRenderer))]
namespace ImageCircle.Forms.Plugin.WindowsStore
{
	/// <summary>
	/// ImageCircle Implementation
	/// </summary>
	public class ImageCircleRenderer : ViewRenderer<Xamarin.Forms.Image, Ellipse>
	{
		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public async static void Init()
		{
			var temp = DateTime.Now;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null || this.Element == null)
				return;

			var ellipse = new Ellipse();
			SetNativeControl(ellipse);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected async override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (Control != null)
			{
				var min = Math.Min(Element.Width, Element.Height) / 2.0f;
				if (min <= 0)
					return;

				try
				{

					Control.Width = Element.Width;
					Control.Height = Element.Height;
					
					// That will be our fallback fill if can't make sense of the ImageSource.
					var color = ((ImageCircle.Forms.Plugin.Abstractions.CircleImage)Element).BorderColor;
					Control.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(
						(byte)(color.A * 255),
						(byte)(color.R * 255),
						(byte)(color.G * 255),
						(byte)(color.B * 255)));


					BitmapImage bitmapImage = null;

					// Handle file images
					if (Element.Source is FileImageSource)
					{
						var fi = Element.Source as FileImageSource;
						var myFile = System.IO.Path.Combine(Package.Current.InstalledLocation.Path, fi.File);
						var myFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Path.GetDirectoryName(myFile));

						using (Stream s = await myFolder.OpenStreamForReadAsync(System.IO.Path.GetFileName(myFile)))
						{
							var memStream = new MemoryStream();
							await s.CopyToAsync(memStream);
							memStream.Position = 0;
							bitmapImage = new BitmapImage();
							bitmapImage.SetSource(memStream.AsRandomAccessStream());
						}

					}

					// Handle uri images
					if (Element.Source is UriImageSource)
						bitmapImage = new BitmapImage((Element.Source as UriImageSource).Uri);

					if (bitmapImage != null)
						Control.Fill = new ImageBrush() { ImageSource = bitmapImage, Stretch = Stretch.UniformToFill };
				}
				catch
				{
					System.Diagnostics.Debug.WriteLine("Unable to create cicrle image");
				}
			}

		}
	}
}