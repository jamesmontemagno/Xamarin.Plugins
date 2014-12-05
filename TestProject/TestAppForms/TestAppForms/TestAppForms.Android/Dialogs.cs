using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestAppForms.Droid;


[assembly: Xamarin.Forms.Dependency(typeof(Dialogs))]
namespace TestAppForms.Droid
{
	public class Dialogs : IDialogs
  {
		public void DisplayActionSheet (string title, string cancelText, string[] items, Action<int> callback)
		{
			var builder = new AlertDialog.Builder(Xamarin.Forms.Forms.Context);
			builder.SetTitle (title)
				.SetItems (items, (sender, args) => {
				if (callback != null)
					callback(args.Which);
				})
				.SetNegativeButton (cancelText, delegate {});

			var dialog = builder.Create();
			dialog.Show ();
		}

  }
}
