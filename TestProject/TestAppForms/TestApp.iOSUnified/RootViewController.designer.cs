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

namespace TestApp.iOSUnified
{
	[Register ("RootViewController")]
	partial class RootViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonDoStuff { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelDidStuff { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ButtonDoStuff != null) {
				ButtonDoStuff.Dispose ();
				ButtonDoStuff = null;
			}
			if (LabelDidStuff != null) {
				LabelDidStuff.Dispose ();
				LabelDidStuff = null;
			}
		}
	}
}
