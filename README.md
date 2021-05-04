# Plugins for .NET, Xamarin, and Windows

Here is my essential list of official and community plugins and libraries for applications. You will find a lot of Xamarin and Xamarin.Forms libraries, but also great libraries for .NET that can help power your apps. If you have a favorite library or are the creator of one, please send a PR!

## [Xamarin.Essentials](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=friends-0000-jamont): The stuff your app needs

Xamarin.Essentials gives developers essential cross-platform APIs for their mobile applications. Xamarin.Essentials exposes over 60 native APIs in a single cross-platform package for developers to consume in their iOS, Android, UWP, or Xamarin.Forms application. Browse through the [documentation](https://docs.microsoft.com/xamarin/essentials?WT.mc_id=friends-0000-jamont) on how to get started today.

The repository for Xamarin.Essentials can be found at https://github.com/xamarin/Essentials. If you have any suggestions or feature requests, or if you find any issues, please open a new issue.

If you have bene using my plugins, I will continue to support the ones that aren't in Xamarin.Essentials, but I recommend you transition using my [guide](https://montemagno.com/upgrading-from-plugins-to-xamarin-essentials/).

## [Shiny](https://www.github.com/shinyorg/shiny): That complex stuff made easy

Shiny is an amazing project from [Allan Ritchie](https://github.com/aritchie), Microsoft MPV and long time Xamarin developer, that makes really complex native functionality easy. This includes things such at Bluetooth LE, Background Jobs, HTTP Transers, Push Notifications, NFC, and more. 

## Community Provided Open Source Plugins

Plugins for Xamarin are community built NuGet and Components that add cross-platform functionality or abstracts platform specific functionality to a common API. These are both completely cross-platform and extremely small (i.e., they do 1 or 2 things really well with minimal-to-no dependencies). The Plugin API can be accessed on each platform, however, you will most likely only use the common API in a .NET Standard project.

> **Notice**: Plugins for Xamarin featured here are produced by the amazing open source community of Xamarin developers. Xamarin does not provide official support for these plugins, please contact their creator with any issues.

Browse through the most popular plugins out there today!

|Name|Description|NuGet|Docs & Source|Creator|
| ------------------- | --------------------------------- | :-----------: | :-----------: |-------------------- |
|Audio Recorder|Record audio on a device's microphone input.|[NuGet](https://www.nuget.org/packages/Plugin.AudioRecorder)|[GitHub](https://github.com/NateRickard/Plugin.AudioRecorder)|[@NateRickard](https://github.com/NateRickard)|
|Barcode Scanner|Scan and create barcodes with ZXing.NET.Mobile.|[NuGet](https://www.nuget.org/packages/ZXing.Net.Mobile)|[GitHub](https://github.com/Redth/ZXing.Net.Mobile)|[@Redth](https://twitter.com/redth)|
|Breach Detector|Detect root, emulation, debug mode and other security concerns.|[NuGet](https://www.nuget.org/packages/Plugin.BreachDetector/)|[GitHub](https://github.com/nmilcoff/BreachDetector)|[@nmilcoff](https://twitter.com/nmilcoff)|
|Calendar|Query and modify device calendars|[NuGet](https://www.nuget.org/packages/CClarke.Plugin.Calendars)|[GitHub](https://github.com/TheAlmightyBob/Calendars/)|[Caleb Clarke](https://github.com/TheAlmightyBob)|
|Config|Define the settings of the application for each environment in which it will run.|[NuGet](https://www.nuget.org/packages/Xamarin.ConfigPlugin/)|[GitHub](https://github.com/AgustinBonilla/ConfigPlugin)|[@abonilla93](https://twitter.com/abonilla93)|
|Custom Vision|Runs CoreML and TensorFlow models from https://CustomVision.ai on device|[NuGet](https://www.nuget.org/packages/Xam.Plugins.OnDeviceCustomVision/)|[GitHub](https://github.com/jimbobbennett/Xam.Plugins.OnDeviceCustomVision)|[@JimBobBennett](https://twitter.com/jimbobbennett)|
|File Picker|Pick and save files.|[NuGet](https://www.nuget.org/packages/Xam.Plugin.FilePicker)|[GitHub](https://github.com/Studyxnet/FilePicker-Plugin-for-Xamarin-and-Windows)|[@studyxnet](http://www.twitter.com/studyxnet)|
|File Uploader|Upload files using multipart request.|[NuGet](https://www.nuget.org/packages/Plugin.FileUploader/)|[GitHub](https://github.com/CrossGeeks/FileUploaderPlugin)|[@rdelrosario](http://www.twitter.com/rdelrosario)|
|Fingerprint|Access Fingerprint sensor on iOS, Android, and Windows.|[NuGet](https://www.nuget.org/packages/Plugin.Fingerprint/)|[GitHub](https://github.com/smstuebe/xamarin-fingerprint)|[@smstuebe](https://github.com/smstuebe)|
|Geolocator|Easily detect GPS location of device.|[NuGet](https://www.nuget.org/packages/Xam.Plugin.Geolocator/)|[GitHub](https://github.com/jamesmontemagno/GeolocatorPlugin)|[@JamesMontemagno](http://www.twitter.com/jamesmontemagno)|
|Identity Document Scanning|Enable scanning of various identity documents in your app.|[NuGet](https://www.nuget.org/packages/BlinkID.Forms/)|[GitHub](https://github.com/blinkid/blinkid-xamarin)|[@microblink](https://twitter.com/microblink)|
|In-App Billing|Make, Query, and get In-App Purchases and Subscriptions.|[NuGet](https://www.nuget.org/packages/Plugin.InAppBilling/)|[GitHub](https://github.com/jamesmontemagno/InAppBillingPlugin)|[@JamesMontemagno](http://www.twitter.com/jamesmontemagno)|
|Media|Take or pick photos and videos.|[NuGet](https://www.nuget.org/packages/Xam.Plugin.Media/)|[GitHub](https://github.com/jamesmontemagno/MediaPlugin)|[@JamesMontemagno](http://www.twitter.com/jamesmontemagno)|
|Media Manager|Playback for Audio and Video.|[NuGet](https://www.nuget.org/packages/Plugin.MediaManager/)|[GitHub](https://github.com/martijn00/XamarinMediaManager)|[@mhvdijk](https://twitter.com/mhvdijk)|
|Multilingual Plugin|Simple cross platform plugin for handling language localization.|[NuGet](https://www.nuget.org/packages/Plugin.Multilingual)|[GitHub](https://github.com/CrossGeeks/MultilingualPlugin)|[@CrossGeeks](https://github.com/CrossGeeks/)|
|Screenshot|Get and save a screenshot of your apps.|[NuGet](https://www.nuget.org/packages/Xam.Plugin.Screenshot/)|[GitHub](https://github.com/wilsonvargas/ScreenshotPlugin)|[@wilson_vargasm](https://twitter.com/Wilson_VargasM)|
|Speech Recognition|Speech to Text.|[NuGet](https://www.nuget.org/packages/Plugin.SpeechRecognition/)|[GitHub](https://github.com/aritchie/speechrecognition)|[@allanritchie911](https://twitter.com/allanritchie911)|
|Simple Audio Player|Play multiple MP3 or wave files from a shared library.|[NuGet](https://www.nuget.org/packages/Xam.Plugin.SimpleAudioPlayer/)|[GitHub](https://github.com/adrianstevens/Xamarin-Plugins/tree/master/SimpleAudioPlayer)|[@adrianstevens](https://github.com/adrianstevens)|
|Store Review|Ask for store review or launch app store page.|[NuGet](https://www.nuget.org/packages/Plugin.StoreReview/)|[GitHub](https://github.com/jamesmontemagno/StoreReviewPlugin)|[@jamesmontemagno](https://github.com/jamesmontemagno)|
|Toast|A simple way of showing toast/pop-up notifications.|[NuGet](https://www.nuget.org/packages/Toasts.Forms.Plugin)|[GitHub](https://github.com/EgorBo/Toasts.Forms.Plugin)|[@AdamPed](https://github.com/AdamPed) & [@EgorBo](https://github.com/EgorBo)|
|User Dialogs|Message box style dialogs.|[NuGet](https://www.nuget.org/packages/Acr.UserDialogs/)|[GitHub](https://github.com/aritchie/userdialogs)|[@allanritchie911](https://twitter.com/allanritchie911)|

## Data Caching & Databases

|Name|Description|NuGet|Docs & Source|Creator|
| ------------------- | --------------------------------- | :-----------: | :-----------: |-------------------- |
|Akavache|An asynchronous, persistent (i.e. writes to disk) key-value store.|[NuGet](https://www.nuget.org/packages/akavache/)|[GitHub](https://github.com/akavache/Akavache)|[@paulcbetts](http://www.twitter.com/paulcbetts)|
|Cosmos DB|Azure Cosmos DB is a globally distributed, multi-model database service.|[NuGet](https://www.nuget.org/packages/Microsoft.Azure.DocumentDB.Core/)|[GitHub](https://github.com/Azure/azure-documentdb-dotnet)|[@azurecosmosdb](http://www.twitter.com/azurecosmosdb)|
|LiteDB|LiteDB - A .NET NoSQL Document Store in a single data file|[NuGet](https://www.nuget.org/packages/LiteDB/)|[GitHub](https://github.com/mbdavid/LiteDB)|[@mbdavid](https://github.com/mbdavid)|
|🐒 Monkey Cache|Cache any data structure for a specific amount of time with minimal dependencies.|[NuGet](https://www.nuget.org/packages?q=monkeycache)|[GitHub](https://github.com/jamesmontemagno/monkey-cache)|[@jamesmontemagno](https://github.com/jamesmontemagno)|
|Mono.Data.Sqlite|Add Mono.Data.Sqlite to any Xamarin or Windows .NET app.|[NuGet](https://www.nuget.org/packages/Mono.Data.Sqlite.Portable)|[GitHub](https://github.com/mattleibow/Mono.Data.Sqlite)|[@mattleibow](https://twitter.com/mattleibow)|
|Realm|Realm is a mobile database: a replacement for SQLite and ORMs.|[NuGet](https://www.nuget.org/packages/Realm.Database/)|[GitHub](https://github.com/realm/realm-dotnet)|[@realm](https://twitter.com/realm)|
|SQLite-net|Simple, powerful, cross-platform SQLite client and ORM for .NET|[NuGet](https://www.nuget.org/packages/sqlite-net-pcl)|[GitHub](https://github.com/praeclarum/sqlite-net)|[@praeclarum](https://twitter.com/praeclarum)|

## Awesome .NET Libraries For Apps
These are just some awesome libraries that work in any .NET app!

|Name|Description|NuGet|Docs & Source|Creator|
| ------------------- | --------------------------------- | :-----------: | :-----------: |-------------------- |
|AnyBind|Easily bind ViewModel to one or more Models.|[NuGet](https://www.nuget.org/packages/AnyBind/)|[GitHub](https://github.com/AlexKven/AnyBind)|[Alexander Kvenvolden](https://github.com/AlexKven)|
|Expressive|Expression parsing and evaluation framework|[NuGet](https://www.nuget.org/packages/ExpressiveParser/)|[GitHub](https://github.com/bijington/expressive)|[Shawn Lawrence](https://github.com/bijington)|
|HttpTracer|Simple tracing library to capture HTTP request/responses.|[NuGet](https://www.nuget.org/packages/HttpTracer/)|[GitHub](https://github.com/BSiLabs/HttpTracer)|[BSi Labs](https://github.com/BSiLabs)|
|MvvmHelpers|Collection of small MVVM helpers for app devs.|[NuGet](https://www.nuget.org/packages/Refractored.MvvmHelpers/)|[GitHub](https://github.com/jamesmontemagno/mvvm-helpers)|[Creator](https://twitter.com/jamesmontemagno)|
|Portable Razor|Lightweight implemenation of ASP.NET MVC APIs for mobile.|[NuGet](https://www.nuget.org/packages/PortableRazor/)|[GitHub](https://github.com/xamarin/PortableRazor)|[@JosephHill](http://www.twitter.com/josephhill)|
|Sockets|TCP & UDP Listeners and Clients + UDP multicast.|[NuGet](https://www.nuget.org/packages/rda.SocketsForPCL)|[GitHub](https://github.com/rdavisau/sockets-for-pcl)|[@rdavis_au](http://www.twitter.com/rdavis_au)|
|TinyInsights|Abstraction for one or more analytics providers from shared code.|[NuGet](https://www.nuget.org/packages/TinyInsights/)|[GitHub](https://github.com/TinyStuff/TinyInsights)|[TinyStuff Team](https://github.com/TinyStuff)|

## MVVM Frameworks

Xamarin.Forms and Windows has MVVM built right, but perhaps you want to go farther and need more. Checkout these great libraries to help you out:

* [FreshMvvm](https://github.com/rid00z/FreshMvvm) - A super light Mvvm Framework designed specifically for Xamarin.Forms. It's designed to be Easy, Simple and Flexible.
* [MvvmAtom](https://github.com/sameerkapps/MvvmAtom) - Provides the middle ground by providing the base classes and wiring for most commonly used MVVM feature.
* [MvvmCross](https://www.mvvmcross.com/) - Build clean, pixel perfect, native UIs. Share behavior and business logic in a single codebase across supported platforms, using the Model-View-ViewModel (MVVM) design pattern. 
* [MvvmLight](https://github.com/lbugnion/mvvmlight) - The main purpose of the toolkit is to accelerate the creation and development of MVVM applications in Xamarin.Android, Xamarin.iOS, Xamarin.Forms, Windows 10 UWP, & Windows Presentation Foundation (WPF).
* [Prism](https://prismlibrary.com/) - Prism is a framework for building loosely coupled, maintainable, and testable XAML applications in WPF, Windows 10 UWP, and Xamarin.Forms.
* [Xamarin.Forms Mvvm Adaptor](https://github.com/z33bs/xamarin-forms-mvvm-adaptor) - ViewModel-first mvvm framework for Xamarin.Forms. Lightweight, it adapts Xamarin's existing mvvm engine.

## UI Controls - Vendors

Looking for controls for your apps? These vendors have absolutely everything that you need. Many offer free trials, community editions or a free version.

* [Aurora Controls](https://www.AuroraControls.app)
* [DevExpress](https://www.devexpress.com/xamarin/)
* [eliteKit](https://www.eliteKit.io)
* [Infragistics](https://www.infragistics.com/products/xamarin)
* [GrapeCity](https://www.grapecity.com/componentone/xamarin-ui-controls)
* [GrialKit](https://www.GrialKit.com)
* [Steema](https://www.steema.com/product/forms)
* [Syncfusion](https://www.syncfusion.com/xamarin-ui-controls)
* [Telerik](https://www.telerik.com/xamarin-ui)

Checkout the awesome trailer for these controls on [YouTube](https://www.youtube.com/watch?v=aXxKy-KjYfM).

## UI Controls - Community

Here are some awesome UI controls for your Xamarin.Forms apps!

|Name|Description|NuGet|Docs & Source|Creator|
| ------------------- | --------------------------------- | :-----------: | :-----------: |-------------------- |
|AutoForms|Auto generated Xamarin.Forms UI from your Domain Models|[NuGet](https://www.nuget.org/packages/Xamarin.Forms.AutoForms)|[GitHub](https://github.com/patrickabadi/AutoForms)|[Patrick Abadi](https://github.com/patrickabadi), [Daniel Packard](https://github.com/daniel-packard)|
|AutoSuggestBox|Auto-complete/suggestion textbox control|[NuGet](https://www.nuget.org/packages/dotMorten.Xamarin.Forms.AutoSuggestBox)|[GitHub](https://github.com/dotMorten/XamarinFormsControls/tree/main/AutoSuggestBox)|[dotMorten](https://github.com/dotMorten)|
|Bit controls|Multi calendar date time picker based on noda time and a few other controls.|[NuGet](https://www.nuget.org/packages/Bit.Client.Xamarin.Controls/)|[GitHub](https://github.com/bitfoundation/bitframework/blob/master/docs/bit-csharp-client/csharp-client-intro.md)|[Bit foundation](https://github.com/bitfoundation)|
|Breadcrumb|Automatically generated breadcrumb navigation control for Xamarin.Forms.|[NuGet](https://www.nuget.org/packages/Xamarin.Forms.Breadcrumb)|[GitHub](https://github.com/IeuanWalker/Xamarin.Forms.Breadcrumb)|[Ieuan Walker](https://twitter.com/IeuanTWalker)| 
|Color Picker|An interactive and responsive Color Picker Control for Xamarin.Forms based on SkiaSharp!|[NuGet](https://www.nuget.org/packages/Udara.Plugin.XFColorPickerControl/)|[GitHub](https://github.com/UdaraAlwis/XFColorPickerControl)|[Udara Alwis](https://github.com/UdaraAlwis)|
|Debug Rainbows|Overlay grids to help manage layouts.|[NuGet](https://www.nuget.org/packages/Xamarin.Forms.DebugRainbows/)|[GitHub](https://github.com/sthewissen/Xamarin.Forms.DebugRainbows)|[Steven Thewissen](https://twitter.com/devnl)|
|FFImageLoading|Image loading with caching, placeholders, transformations and more|[NuGet](https://www.nuget.org/packages/Xamarin.FFImageLoading.Forms/)|[GitHub](https://github.com/molinch/FFImageLoading)|[@molinch](https://github.com/molinch), [@daniel-luberda](https://github.com/daniel-luberda/)|
|Glidex.Forms|Glidex.Forms is a library using Glide for faster Xamarin.Forms images on Android.|[NuGet](https://www.nuget.org/packages/glidex.forms)|[GitHub](https://github.com/jonathanpeppers/glidex)|[@jonathanpeppers](https://github.com/jonathanpeppers/)|
|FloatingActionMenu|Floating action menu, inspired by the material design floating action button. |[NuGet](https://www.nuget.org/packages/DIPS.Xamarin.UI/)|[GitHub](https://github.com/DIPSAS/DIPS.Xamarin.UI)|[DIPS AS Team](https://github.com/DIPSAS)|
|MagicGradients|Gradients everywhere! Powered by SkiaSharp|[NuGet](https://www.nuget.org/packages/MagicGradients)|[GitHub](https://github.com/mgierlasinski/MagicGradients)|[Marcin Gierlasiński](https://github.com/mgierlasinski)|
|Material Design Controls for Xamarin.Forms|The suite of Material Design controls with a feature-rich. Create fast, beautiful, and cross-platform mobile apps.|[NuGet](https://www.nuget.org/packages/Plugin.MaterialDesignControls)|[GitHub](https://github.com/HorusSoftwareUY/MaterialDesignControlsPlugin)|[Horus Studio](https://github.com/HorusSoftwareUY)|
|MtAdmob|Add Google Admob banners, interstitials and rewarded videos to Android and iOS with a single line of code.|[NuGet](https://www.nuget.org/packages/MarcTron.Admob/)|[GitHub](https://github.com/marcojak/MTAdmob)|[@marcojak](https://github.com/marcojak/)|
|PancakeView|An extended ContentView for Xamarin.Forms with rounded corners, borders, shadows and more.|[NuGet](https://nuget.org/packages/Xamarin.Forms.PancakeView)|[GitHub](https://github.com/sthewissen/Xamarin.Forms.PancakeView)|[Steven Thewissen](https://twitter.com/devnl)|
|Popup|Custom popups with a nice modality feed to it.|[NuGet](https://www.nuget.org/packages/DIPS.Xamarin.UI/)|[GitHub](https://github.com/DIPSAS/DIPS.Xamarin.UI)|[DIPS AS Team](https://github.com/DIPSAS)|
|Shared Transitions|Des|[NuGet](https://www.nuget.org/packages/Xamarin.Plugin.SharedTransitions/)|[GitHub](https://github.com/GiampaoloGabba/Xamarin.Plugin.SharedTransitions)|[Giampaolo Gabba](https://github.com/GiampaoloGabba)|
|Sheet|Sliding bottom / top sheet control.|[NuGet](https://www.nuget.org/packages/DIPS.Xamarin.UI/)|[GitHub](https://github.com/DIPSAS/DIPS.Xamarin.UI)|[DIPS AS Team](https://github.com/DIPSAS)|
|Sharpnado|Collection of controls including Tabs, TaskLoaderVIew, and more. |[NuGet](https://www.nuget.org/packages?q=sharpnado)|[GitHub](https://github.com/roubachof/Sharpnado.Presentation.Forms)|[Jean-Marie Alfonsi](https://www.sharpnado.com/)|
|Skeleton for Xamarin.Forms|The latest trend for loading approaches in Xamarin Forms apps.|[NuGet](https://www.nuget.org/packages/Xamarin.Forms.Skeleton)|[GitHub](https://github.com/HorusSoftwareUY/Xamarin.Forms.Skeleton)|[Horus Studio](https://github.com/HorusSoftwareUY)|
|SkiaSharp|Cross-platform 2D graphics API for .NET powered by Skia|[NuGet](https://www.nuget.org/packages/SkiaSharp)|[GitHub](https://github.com/mono/SkiaSharp)|[Mono Team](https://github.com/mono)|
|SkiaSharp for Xamarin.Forms|Cross-platform 2D graphics API for Xamarin.Forms powered by Skia|[NuGet](https://www.nuget.org/packages/SkiaSharp.Views.Forms)|[GitHub](https://github.com/mono/SkiaSharp)|[Mono Team](https://github.com/mono)|
|StateButton|With this control you are able to create any style of button. This is possible as it acts as a wrapper to your XAML and provides you the events/ commands and properties to bind too.|[Nuget](https://www.nuget.org/packages/IeuanWalker.StateButton)|[GitHub](https://github.com/IeuanWalker/Xamarin.Forms.StateButton)|[Ieuan Walker](https://twitter.com/IeuanTWalker)|
|StateSquid|Turn any layout into an individual state-aware element|[Nuget](https://www.nuget.org/packages/Xamarin.Forms.StateSquid)|[GitHub](https://github.com/sthewissen/Xamarin.Forms.StateSquid)|[Steven Thewissen](https://twitter.com/devnl)|
|Switch|A switch control that allows you to design/ create any switch you want|[Nuget](https://www.nuget.org/packages/IeuanWalker.CustomSwitch)|[GitHub](https://github.com/IeuanWalker/Xamarin.Forms.CustomSwitch)|[Ieuan Walker](https://twitter.com/IeuanTWalker)|
|TouchEffect|Make Xamarin.Forms views touch-responsive without TapGestureRecognizer.|[NuGet](https://www.nuget.org/packages/TouchView)|[GitHub](https://github.com/AndreiMisiukevich/TouchEffect)|[Andrei Misiukevich](https://twitter.com/Andrik_Just4Fun)|
|TrendGraph|Display a trend graph in your mobile app. |[NuGet](https://www.nuget.org/packages/DIPS.Xamarin.UI/)|[GitHub](https://github.com/DIPSAS/DIPS.Xamarin.UI)|[DIPS AS Team](https://github.com/DIPSAS)|
|Xamanimation|An awesome animation library for Xamarin.Forms|[NuGet](https://www.nuget.org/packages/Xamanimation/)|[GitHub](https://github.com/jsuarezruiz/Xamanimation)|[Creator]()|
|Xamlly|Control library featuring progress, switch, toggle button, and more.|[NuGet](https://www.nuget.org/packages/Xamlly/)|[GitHub](https://github.com/mshwf/Xamlly)|[Mohamed Elshawaf](https://github.com/mshwf)|

## Great Utilities
These are things that your app needs, but aren't UI or normal libraries such as Visual Studio extensions or other magic build time things.

|Name|Description|NuGet|Docs & Source|Creator|
| ------------------- | --------------------------------- | :-----------: | :-----------: |-------------------- |
|Mobile Tasks for Azure DevOps|Tasks to update version numbers & package information|[VS Marketplace](https://marketplace.visualstudio.com/items?itemName=vs-publisher-473885.motz-mobile-buildtasks&WT.mc_id=xamarin.plugins-github-jamont)|[GitHub](https://github.com/jamesmontemagno/vsts-mobile-tasks)|[James Montemagno](https://www.twitter.com/jamesmontemagno)|
|Mobile.BuildTools|Handle Project Secrets, Process SCSS to Xamarin.Forms CSS, Cross platform image management, White-Labeling and more powered by MSBuild.|[NuGet](https://https://www.nuget.org/packages/Mobile.BuildTools)|[GitHub](https://github.com/dansiegel/Mobile.BuildTools)|[Dan Siegel](https://twitter.com/DanJSiegel)|
|ResizetizerNT|Add & Resize SVGs and PNGs to your shared projects.|[NuGet](https://www.nuget.org/packages/Resizetizer.NT)|[GitHub](https://github.com/Redth/ResizetizerNT)|[Jon Dick](https://twitter.com/redth)|
|VSMac-CodeCoverage|Gather code coverage results for your unit test projects from Visual Studio for Mac.|[mpack install](https://github.com/ademanuele/VSMac-CodeCoverage/releases)|[GitHub](https://github.com/ademanuele/VSMac-CodeCoverage)|[Arthur Demanuele](https://twitter.com/arthurdemanuele)|
|VSMac-CodeDistribution|A Visual Studio for Mac extension that visualises code distribution between projects. Particularly useful for Xamarin projects to understand the amount of code shared between platforms.|[mpack install](https://github.com/ademanuele/VSMac-CodeDistribution/releases)|[GitHub](https://github.com/ademanuele/VSMac-CodeDistribution)|[Arthur Demanuele](https://twitter.com/arthurdemanuele)|


## Create a Plugin for Xamarin
If you are looking to create a plugin be sure to browse through NuGet first and ensure that the plugin doesn't exist. If one does join in on the fun and collaborate. If it doesn't and you want to start building a Plugin here are some tools and guidelines to get you started.

**Tools to get Started**
* [Visual Studio Plugin Templates](https://visualstudiogallery.msdn.microsoft.com/afead421-3fbf-489a-a4e8-4a244ecdbb1e?WT.mc_id=friends-0000-jamont): Provides a complete plugin template and easy NuGet specification to publish.
* [Using & Developing Plugins for Xamarin](https://university.xamarin.com/guestlectures/using-developing-plugins-for-xamarin): Join Developer Evangelist James Montemagno as he walks you through creating a plugin from scratch on Xamarin University

**Requirements of a Plugin**
* Open source on GitHub
* Documentation on GitHub's README file
* Name: "FEATURE_NAME Plugin for Xamarin and Windows"
* Namespace: Plugin.FEATURE_NAME
* App-store friendly OSS license (we like MIT)
* No dependency on Xamarin.Forms
* Have a list of supported and unsupported OSs in its GitHub wiki


#### License
Licensed under MIT see License file. Each plugin licensed under parent license unless stated in it's readme file.

### Want To Support This Project?
All I have ever asked is to be active by submitting bugs, features, and sending those pull requests down! Want to go further? Make sure to subscribe to my weekly development podcast [Merge Conflict](http://mergeconflict.fm), where I talk all about awesome Xamarin goodies and you can optionally support the show by becoming a [supporter on Patreon](https://www.patreon.com/mergeconflictfm).
