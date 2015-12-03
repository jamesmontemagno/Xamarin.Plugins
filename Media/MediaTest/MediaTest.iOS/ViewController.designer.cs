// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MediaTest.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton PickPhoto { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton PickVideo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton TakePhoto { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton TakeVideo { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (PickPhoto != null) {
				PickPhoto.Dispose ();
				PickPhoto = null;
			}
			if (PickVideo != null) {
				PickVideo.Dispose ();
				PickVideo = null;
			}
			if (TakePhoto != null) {
				TakePhoto.Dispose ();
				TakePhoto = null;
			}
			if (TakeVideo != null) {
				TakeVideo.Dispose ();
				TakeVideo = null;
			}
		}
	}
}
