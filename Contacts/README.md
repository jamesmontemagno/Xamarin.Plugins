## ContactsPlugin for Xamarin and Windows

Simple cross platform plugin to get Contacts from the device.

Ported from [Xamarin.Mobile](http://www.github.com/xamarin/xamarin.mobile) to a cross platform API.

### Setup
* Currently in Alpha (turn on pre-release packages)
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.Contacts
* Install into your PCL project and Client projects.

**Supports**
* Xamarin.iOS
* Xamarin.iOS (x64 Unified)
* Xamarin.Android
* Windows Phone 8 (Silverlight)

### API Usage Example
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




### Important

**Android**
You must add android.permissions.READ_CONTACTS

**Windows Phone**
You must add ID_CAP_CONTACTS permission

#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under main repo license
