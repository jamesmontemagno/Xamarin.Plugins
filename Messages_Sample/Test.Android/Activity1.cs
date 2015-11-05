using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Test.Portable;

namespace Test.Android
{
    [Activity(Label = "Test.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var button1 = FindViewById<Button>(Resource.Id.button1);
            var button2 = FindViewById<Button>(Resource.Id.button2);

            button1.Click += delegate
            {
                Class1.ShowMessage("from Android");
            };
            button2.Click += delegate
            {
                Class1.Context = this;
                Class1.ShowMessageEx("Title", "from Android");
            };

        }
    }
}

