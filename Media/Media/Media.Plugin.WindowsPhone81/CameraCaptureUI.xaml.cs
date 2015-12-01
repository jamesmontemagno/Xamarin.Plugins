using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DMX.Helper
{

    internal enum CameraCaptureUIMode
    {
        PhotoOrVideo,
        Photo,
        Video
    }

    internal sealed partial class CameraCaptureUI : UserControl
    {
        // store the pic here
        StorageFile file;

        // stop flag - needed to find when to get back to former page
        bool stopFlag = false;

        public bool StopFlag
        {
            get { return stopFlag; }
            set { stopFlag = value; }
        }

        // the root grid of our camera ui page
        Grid mainGrid;

        private MediaCapture mediaCapture;
        public MediaCapture MyMediaCapture
        {
            get
            {
                return mediaCapture;
            }
            set { mediaCapture = value; }
        }
        Frame originalFrame;
        private const short WaitForClickLoopLength = 1000;


        /// <summary>
        /// Navigates to the CameraCaptureUIPage in a new Frame and show the control
        /// </summary>
        public CameraCaptureUI()
        {
            InitializeComponent();

            // get current app
            app = Application.Current;

            // get current frame
            originalFrame = (Frame)Window.Current.Content;

            CurrentWindow = Window.Current;
            NewCamCapFrame = new Frame();
            CurrentWindow.Content = NewCamCapFrame;

            // navigate to Capture UI page 
            NewCamCapFrame.Navigate(typeof(CameraCaptureUIPage));

            Unloaded += CameraCaptureUI_Unloaded;
#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif

            // set references current CCUI page
            myCcUiPage = ((CameraCaptureUIPage)NewCamCapFrame.Content);

            myCcUiPage.MyCCUCtrl = this;

            app.Suspending += AppSuspending;
            app.Resuming += AppResuming;


            // set content
            mainGrid = (Grid)(myCcUiPage).Content;

            // Remove all children, if any exist
            mainGridChildren = mainGrid.Children;
            foreach (var item in mainGridChildren)
            {
                mainGrid.Children.Remove(item);
            }

            // Show Ctrl
            mainGrid.Children.Add(this);
        }



        public async void AppResuming(object sender, object e)
        {
            // get current frame
            NewCamCapFrame = (Frame)Window.Current.Content;

            // make sure you are on CCUIPage
            var ccuipage = NewCamCapFrame.Content as CameraCaptureUIPage;
            if (ccuipage != null)
            {
                var ccu = ccuipage.MyCCUCtrl;

                // start captureing again
                await ccu.CaptureFileAsync(CameraCaptureUIMode.Photo, options);
            }
            else
            {
                app.Resuming -= AppResuming;
            }
        }

        public async void AppSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await CleanUpAsync();
            deferral.Complete();
        }

        private void CameraCaptureUI_Unloaded(object sender, RoutedEventArgs e)
        {
#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
        }

        async void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            await GoBackAsync(e);
        }

        private async Task GoBackAsync(Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            await CleanUpAsync();

            e.Handled = true;

            CurrentWindow.Content = originalFrame;
        }

        public async Task CleanUpAsync()
        {
            if (myCaptureElement != null)
            {
                myCaptureElement.Source = null;
            }

            if (MyMediaCapture != null)
            {
                try
                {
                    await MyMediaCapture.StopPreviewAsync();
                }
                catch (ObjectDisposedException o)
                {
                    Debug.WriteLine(o.Message);
                }
            }

            if (MyMediaCapture != null)
            {
                try
                {
                    MyMediaCapture.Dispose();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }



        StoreCameraMediaOptions options;
        /// <summary>
        /// This method takes a picture. 
        /// Right now the parameter is not evaluated.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<StorageFile> CaptureFileAsync(CameraCaptureUIMode mode, StoreCameraMediaOptions options)
        {
            var t = IsStopped();
            options = options;
            // Create new MediaCapture 
            MyMediaCapture = new MediaCapture();
            var videoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var backCamera = videoDevices.FirstOrDefault(
                item => item.EnclosureLocation != null
                && item.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

            var frontCamera = videoDevices.FirstOrDefault(
                  item => item.EnclosureLocation != null
                  && item.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);

            var captureSettings = new MediaCaptureInitializationSettings();
            if(options.DefaultCamera == CameraDevice.Front && frontCamera != null)
            {
              captureSettings.VideoDeviceId = frontCamera.Id;
            }
            else if(options.DefaultCamera == CameraDevice.Rear && backCamera != null)
            {
              captureSettings.VideoDeviceId = backCamera.Id; ;
            }
            await MyMediaCapture.InitializeAsync(captureSettings);

            // Assign to Xaml CaptureElement.Source and start preview
            myCaptureElement.Source = MyMediaCapture;

            // show preview
            await MyMediaCapture.StartPreviewAsync();

            // now wait until stopflag shows that someone took a picture
            await t;

            // picture has been taken
            // stop preview

            await CleanUpAsync();

            // go back
            CurrentWindow.Content = originalFrame;

            mainGrid.Children.Remove(this);

            return file;
        }

        /// <summary>
        /// This is a loop which waits async until the flag has been set.
        /// </summary>
        /// <returns></returns>
        async private Task IsStopped()
        {
            while (!StopFlag)
            {
                await Task.Delay(WaitForClickLoopLength);
            }
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create new file in the pictures library     

            file = await ApplicationData.Current.LocalFolder.CreateFileAsync("_____ccuiphoto.jpg", CreationCollisionOption.ReplaceExisting);


            // create a jpeg image
            var imgEncodingProperties = ImageEncodingProperties.CreateJpeg();

            await MyMediaCapture.CapturePhotoToStorageFileAsync(imgEncodingProperties, file);

            // when pic has been taken, set stopFlag
            StopFlag = true;
        }


        public UIElementCollection mainGridChildren { get; set; }

        public bool locker = false;

        public Application app { get; set; }

        private CameraCaptureUIPage myCcUiPage;

        public CameraCaptureUIPage MyCciPage
        {
            get { return myCcUiPage; }
            set { myCcUiPage = value; }
        }

        public Window CurrentWindow { get; set; }

        public Frame NewCamCapFrame { get; set; }
    }
}


