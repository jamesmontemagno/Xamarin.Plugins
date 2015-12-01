using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DMX.Helper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraCaptureUIPage : Page
    {

        DisplayOrientations previous;
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
          DisplayInformation.AutoRotationPreferences = previous;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationCacheMode = NavigationCacheMode.Required;
        }
        
        public CameraCaptureUIPage()
        {
          previous = DisplayInformation.AutoRotationPreferences;
          //DisplayInformation.AutoRotationPreferences = DisplayOrientations.;
            this.Loaded += CameraCaptureUIPage_Loaded;
            this.Unloaded += CameraCaptureUIPage_Unloaded;
            this.InitializeComponent();
        }

        void CameraCaptureUIPage_Unloaded(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;
            app.Suspending -= MyCCUCtrl.AppSuspending;
            app.Resuming -= MyCCUCtrl.AppResuming;
            DisplayInformation.AutoRotationPreferences = previous; 
        }

        void CameraCaptureUIPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>   

        internal CameraCaptureUI MyCCUCtrl { get; set; }
    }
}
