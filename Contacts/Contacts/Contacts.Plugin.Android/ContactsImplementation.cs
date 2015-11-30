using Android.Database;
using Android.Provider;
using Plugin.Contacts.Abstractions;
using Plugin.Permissions;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Plugin.Contacts
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class ContactsImplementation : IContacts
  {
    public async Task<bool> RequestPermission()
    {
        var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permissions.Abstractions.Permission.Contacts).ConfigureAwait(false);
        if (status != Permissions.Abstractions.PermissionStatus.Granted)
        {
            Console.WriteLine("Currently does not have Contacts permissions, requesting permissions");

            var request = await CrossPermissions.Current.RequestPermissionsAsync(Permissions.Abstractions.Permission.Contacts);

            if (request[Permissions.Abstractions.Permission.Contacts] != Permissions.Abstractions.PermissionStatus.Granted)
            {
                Console.WriteLine("Contacts permission denied, can not get positions async.");
                return false;
            }
        }

        return true;
    }

    private AddressBook addressBook;
    public IQueryable<Contact> Contacts
    {
      get 
      {
        return (IQueryable<Contact>)AddressBook;   
      }
    }
    private AddressBook AddressBook
    {
      get
      {
        return addressBook ?? (addressBook = new AddressBook(Android.App.Application.Context));
      }
    }

    public Abstractions.Contact LoadContact(string id)
    {
      return AddressBook.Load(id);
    }

    public bool LoadSupported
    {
      get { return true; }
    }

    public bool PreferContactAggregation
    {
      get
      {
        return AddressBook.PreferContactAggregation;
      }
      set
      {
        AddressBook.PreferContactAggregation = value;
      }
    }

    public bool AggregateContactsSupported
    {
      get { return true; }
    }

    public bool SingleContactsSupported
    {
      get { return true; }
    }

    public bool IsReadOnly
    {
      get { return true; }
    }
  }
}