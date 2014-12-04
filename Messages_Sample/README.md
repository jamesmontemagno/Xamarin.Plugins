## Messages 

### What is it?
* An example of showing a pop up dialog on different platforms.
* This is just an example PCL that you can use as a reference when creating new PCLs with platform specific code.




### NuGet Creation & Packaging

This NuGet takes a different approach from Settings. I simply created 2 classes:

* Messages.cs
* MessagesEx.cs

Inside of Messages.cs I have stubbed out a method to pop up a message box. I then created the same Message.cs file in each platform specific implementation.

I also wanted to show an example of a linked file, wich is MessagesEx.cs. I created a class with a single method and then linked it to each project. This allows all implementation to be done in 1 file. This then allows me to do if defs to implement each platform.

You can find my nuspec here: https://github.com/jamesmontemagno/Xam.PCL.Plugins/blob/master/Messages/Common/Xam.Plugins.Messages.nuspec


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Main license of repo
