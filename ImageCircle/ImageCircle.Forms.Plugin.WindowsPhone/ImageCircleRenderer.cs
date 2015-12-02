using ImageCircle.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.WindowsPhone;
using Xamarin.Forms.Platform.WinPhone;
using System.Windows.Media;
using System.Windows.Shapes;

[assembly: ExportRenderer(typeof(CircleImage), typeof(ImageCircleRenderer))]
namespace ImageCircle.Forms.Plugin.WindowsPhone
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    public class ImageCircleRenderer : ImageRenderer
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public async static void Init()
        {
            var temp = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
           

            if (Control != null && Control.Clip == null)
            {
                var min = Math.Min(Element.Width, Element.Height) / 2.0f;
                if (min <= 0)
                    return;

                var clip = new EllipseGeometry
                {
                    Center = new System.Windows.Point(min, min),
                    RadiusX = min,
                    RadiusY = min,
                };
                
                Control.Clip = clip;
            }
        }
    }
}
