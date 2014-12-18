using ImageCircle.Forms.Plugin.Abstractions;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using ImageCircle.Forms.Plugin.Droid;
using System;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(ImageCircle.Forms.Plugin.Abstractions.CircleImage), typeof(ImageCircleRenderer))]
namespace ImageCircle.Forms.Plugin.Droid
{
  /// <summary>
  /// ImageCircle Implementation
  /// </summary>
  public class ImageCircleRenderer : ImageRenderer
  {
    /// <summary>
    /// Used for registration with dependency service
    /// </summary>
    public static void Init() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
    {
      base.OnElementChanged(e);

      if (e.OldElement == null)
      {

        if ((int)Android.OS.Build.VERSION.SdkInt < 18)
          SetLayerType(LayerType.Software, null);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="child"></param>
    /// <param name="drawingTime"></param>
    /// <returns></returns>
    protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
    {
      try
      {
        var radius = Math.Min(Width, Height) / 2;
        var strokeWidth = 10;
        radius -= strokeWidth / 2;


        Path path = new Path();
        path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
        canvas.Save();
        canvas.ClipPath(path);

        var result = base.DrawChild(canvas, child, drawingTime);

        canvas.Restore();

        path = new Path();
        path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

        var paint = new Paint();
        paint.AntiAlias = true;
        paint.StrokeWidth = ((ImageCircle.Forms.Plugin.Abstractions.CircleImage)Element).BorderThickness;
        paint.SetStyle(Paint.Style.Stroke);
        paint.Color = ((ImageCircle.Forms.Plugin.Abstractions.CircleImage)Element).BorderColor.ToAndroid();

        canvas.DrawPath(path, paint);

        paint.Dispose();
        path.Dispose();
        return result;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
      }

      return base.DrawChild(canvas, child, drawingTime);
    }
  }
}
