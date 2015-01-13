using Android.Database;
using Android.Provider;
using Contacts.Plugin.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Contacts.Plugin
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class ContactsImplementation : IContacts
  {
    public Task<bool> RequestPermission()
    {
      return Task.Factory.StartNew(() =>
      {
        try
        {
          var cursor = Android.App.Application.Context.ContentResolver.Query(ContactsContract.Data.ContentUri, null, null, null, null);
          cursor.Close();
          cursor.Dispose();

          return true;
        }
        catch (Java.Lang.SecurityException)
        {
          return false;
        }
      });
    }

    private AddressBook addressBook;
    public IQueryable<Abstractions.Contact> Contacts
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