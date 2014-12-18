## Xamarin.Plugins for Xamarin and Windows Projects

## What is this?
This is my main repo for all all Xamarin Plugins that I have or will be publishing. It is also example and documentation on how to create your very own PCL Plugin. These PCL plugins will allow you to add rich cross platform functionality to Xamarin, Xamarin.Forms and Windows Projects that use a PCLs or Shared Project.

Read more on my blog: http://motzcod.es/post/104507063997/what-exactly-is-a-plugin-for-xamarin

# Current Plugins for Xamarin

Each plugin has a README with more information on what they contain.
* **[Connectivity](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Connectivity)**: See if device is connected to the internet and through what connection type.
* **[Device Information](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/DeviceInfo)**
* **[Settings](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Settings)**
* **[Text To Speech](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/TextToSpeech)**
* **[Vibrate](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Vibrate)**

Controls:
* **[Circle Image](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle)** for Xamarin.Forms

Coming soon:
* **Messages_Production**: In the works

Other:
* **Messages_Sample**: This is a simple test PCL NuGet to show anyone how easy it is to get a NuGet up and running. It uses an example of a stubbed out class and also a linked file class with #if's throughout it.


## How PCL's Work!

PCL's consist of 2 concepts:

* Reference Assembly (API Contract)
* Implementation

Usually these 2 concepts exist together in one single assembly, however PCL's are special and allow you to separate your Reference Assembly from your implementation. This makes it extremely valuable to have platform specific implementations but develop against the same API on all platforms.

### Compile vs Deploy

When we are creating a PCL library our PCL project will contain the API Contract with either an Interface that must be implemented on each platform or a stubbed our class that will be implemented on each platform. When we create our NuGet this reference assembly will be installed into our PCL that we can code against. 

We will then create a separate project for each platform that we wish to support and implement the interface or stub class on each of them. The key here is that our reference assembly and each platform project have the SAME namespaces & assembly name. When we install into a specific platform via NuGet the platform specific DLL will be referenced.

The magic here is that while we are compiling against our reference assembly in the PCL when we go to deploy our application the reference assembly will be replaced with our platform specific implementation for the specific platform.

This is extremely powerful as long as you as a library creator ensure that your namespaces, assembly names, and all methods are implemented correctly on each platform.

### Packaging up your your PCL

You will want to create a nuspec for your DLLs. I have examples in each of my projects for this. Once you have them done simply follow the guidelines: http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package

You will want to ensure that you follow the naming guidelines for each platform:

Platform specific: 

* MonoTouch10
* MonoAndroid10
* win8
* wpa81
* wp8
* net45
* MonoMac10

New Unified APIs 

* Xamarin.iOS10 
* Xamarin.Mac20

PCL:

* portable-net45+win8+wp8+MonoTouch10+MonoAndroid10+wpa81+Xamarin.iOS10 

#### Important

* Ensure namespaces are the same
* Ensure that Assembly Names are the same. You will see all of mine are called Refractored.Xam.Messages.dll in all projects!


#### Upcoming PCL Plugins
* A real Messages PCL
* SMS

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under MIT see License file
