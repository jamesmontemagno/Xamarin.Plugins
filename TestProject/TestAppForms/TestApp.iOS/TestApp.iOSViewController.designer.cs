// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;

namespace TestApp.iOS
{
	[Register ("TestAppiOSViewController")]
	partial class TestAppiOSViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonDoStuff { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LabelDoStuff { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ButtonDoStuff != null) {
				ButtonDoStuff.Dispose ();
				ButtonDoStuff = null;
			}
			if (LabelDoStuff != null) {
				LabelDoStuff.Dispose ();
				LabelDoStuff = null;
			}
		}
	}
}
