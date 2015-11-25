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

namespace ConnectivityTest.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonRegister { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonStatus { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelStatus { get; set; }

		[Action ("ButtonRegister_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonRegister_TouchUpInside (UIButton sender);

		[Action ("ButtonStatus_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonStatus_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonRegister != null) {
				ButtonRegister.Dispose ();
				ButtonRegister = null;
			}
			if (ButtonStatus != null) {
				ButtonStatus.Dispose ();
				ButtonStatus = null;
			}
			if (LabelStatus != null) {
				LabelStatus.Dispose ();
				LabelStatus = null;
			}
		}
	}
}
