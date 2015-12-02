using System;
using Xamarin.Forms;

namespace ImageCircle.Forms.Plugin.Abstractions
{
    /// <summary>
    /// ImageCircle Interface
    /// </summary>
    public class CircleImage : Image
    {
        /// <summary>
        /// Thickness property of border
        /// </summary>
        public static readonly BindableProperty BorderThicknessProperty =
          BindableProperty.Create<CircleImage, int>(
            p => p.BorderThickness, 0);

        /// <summary>
        /// Border thickness of circle image
        /// </summary>
        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Color property of border
        /// </summary>
        public static readonly BindableProperty BorderColorProperty =
          BindableProperty.Create<CircleImage, Color>(
            p => p.BorderColor, Color.White);

        /// <summary>
        /// Border Color of circle image
        /// </summary>
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        /// <summary>
        /// Color property of fill
        /// </summary>
        public static readonly BindableProperty FillColorProperty =
          BindableProperty.Create<CircleImage, Color>(
            p => p.FillColor, Color.Transparent);

        /// <summary>
        /// Fill color of circle image
        /// </summary>
        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

    }
}
