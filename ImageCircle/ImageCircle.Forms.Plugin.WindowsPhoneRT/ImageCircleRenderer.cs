#if WINDOWS_UWP
using ImageCircle.Forms.Plugin.UWP;
using Xamarin.Forms.Platform.UWP;
#else
using ImageCircle.Forms.Plugin.WindowsPhoneRT;
using Xamarin.Forms.Platform.WinRT;
#endif
using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.Abstractions;

[assembly: ExportRenderer(typeof(CircleImage), typeof(ImageCircleRenderer))]
#if WINDOWS_UWP
namespace ImageCircle.Forms.Plugin.UWP
#else
namespace ImageCircle.Forms.Plugin.WindowsPhoneRT
#endif
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    public class ImageCircleRenderer : ViewRenderer<Image, Ellipse>
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public async static void Init()
        {
            var temp = DateTime.Now;
        }

        /// <summary>
        /// Register circle
        /// </summary>
        /// <param name="e"></param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;

            var ellipse = new Ellipse();
            SetNativeControl(ellipse);

        }

        Xamarin.Forms.ImageSource file = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
                return;



            var min = Math.Min(Element.Width, Element.Height) / 2.0f;
            if (min <= 0)
                return;

            try
            {

                Control.Width = Element.Width;
                Control.Height = Element.Height;

                // Fill background color
                var color = ((CircleImage)Element).FillColor;
                Control.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(
                    (byte)(color.A * 255),
                    (byte)(color.R * 255),
                    (byte)(color.G * 255),
                    (byte)(color.B * 255)));

                // Fill stroke
                color = ((CircleImage)Element).BorderColor;
                Control.StrokeThickness = ((CircleImage)Element).BorderThickness;
                Control.Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(
                   (byte)(color.A * 255),
                   (byte)(color.R * 255),
                   (byte)(color.G * 255),
                   (byte)(color.B * 255)));

                var force = e.PropertyName == VisualElement.XProperty.PropertyName ||
                    e.PropertyName == VisualElement.YProperty.PropertyName ||
                    e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                    e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                    e.PropertyName == VisualElement.ScaleProperty.PropertyName ||
                    e.PropertyName == VisualElement.TranslationXProperty.PropertyName ||
                    e.PropertyName == VisualElement.TranslationYProperty.PropertyName ||
                    e.PropertyName == VisualElement.RotationYProperty.PropertyName ||
                    e.PropertyName == VisualElement.RotationXProperty.PropertyName ||
                    e.PropertyName == VisualElement.RotationProperty.PropertyName ||
                    e.PropertyName == VisualElement.AnchorXProperty.PropertyName ||
                    e.PropertyName == VisualElement.AnchorYProperty.PropertyName;


                //already set
                if (file == Element.Source && !force)
                    return;

                file = Element.Source;

                var handler = GetHandler(file);
                var imageSource = await handler.LoadImageAsync(file);

                if (imageSource != null)
                {
                    Control.Fill = new ImageBrush
                    {
                        ImageSource = imageSource,
                        Stretch = Stretch.UniformToFill,
                    };
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Unable to create circle image, falling back to background color.");
            }
        }

        private static IImageSourceHandler GetHandler(Xamarin.Forms.ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImagesourceHandler();
            }
            return returnValue;
        }

    }
}