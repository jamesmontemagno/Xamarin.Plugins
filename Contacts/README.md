## ContactsPlugin for Xamarin and Windows

Simple cross platform plugin to get Contacts from the device.

Ported from [Xamarin.Mobile](http://www.github.com/xamarin/xamarin.mobile) to a cross platform API.

### Setup
* Currently in Alpha (turn on pre-release packages)
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.Contacts [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.Contacts.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.Contacts/)
* Install into your PCL project and Client projects.

**Platform Support**

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.iOS Unified|Yes|iOS 7+|
|Xamarin.Android|Yes|API 14+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|---|8.1+|
|Windows Store RT|---|8.1+|
|Windows 10 UWP|---|10+|
|Xamarin.Mac|No||

**Supports**
* Xamarin.iOS
* Xamarin.iOS (x64 Unified)
* Xamarin.Android
* Windows Phone 8 (Silverlight)
* Windows Phone 8.1 RT (Blank Implementation)
* Windows Store 8.1 (Blank Implementation)
* Windows 10 UWP (Blank Implementation)

### API Usage Example
```csharp
if(await CrossContacts.Current.RequestPermission())
      {
     
        List<Contact> contacts = null;
        CrossContacts.Current.PreferContactAggregation = false;//recommended
//run in background
        await Task.Run(() =>
        {
          if(CrossContacts.Current.Contacts == null)
            return;

          contacts = CrossContacts.Current.Contacts
            .Where(c => !string.IsNullOrWhiteSpace(c.LastName) && c.Phones.Count > 0)         
            .ToList();

          contacts = contacts.OrderBy(c => c.LastName).ToList();
        });
      }
```



### Important

**Android**
Tha android.permissions.READ_CONTACTS permission is required, but the library will automatically add this for you. Additionally, if your users are running Marshmallow the Plugin will automatically prompt them for runtime permissions when RequestPermissions() is called.

**iOS**
Permissions will automatically be requrested when RequestPermissions() is called.

**Windows Phone**
You must add ID_CAP_CONTACTS permission

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
This is a derivative to [Xamarin.Mobile's Contacts](http://github.com/xamarin/xamarin.mobile) with a cross platform API and other enhancements.
﻿//
//  Copyright 2011-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
