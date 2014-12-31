using Contacts.Plugin.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Contacts.Plugin
{
  /// <summary>
  /// Implementation for Contacts
  /// </summary>
  public class ContactsImplementation : IContacts
  {
    public Task<bool> RequestPermission()
    {
      return Task<bool>.Factory.StartNew(() =>
      {
        try
        {
          var contacts = new Microsoft.Phone.UserData.Contacts();
          contacts.Accounts.ToArray(); // Will trigger exception if manifest doesn't specify ID_CAP_CONTACTS
          return true;
        }
        catch (Exception)
        {
          return false;
        }
      }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
    }

    private AddressBook addressBook;
    private AddressBook AddressBook
    {
      get { return addressBook ?? (addressBook = new AddressBook()); }
    }
    public System.Linq.IQueryable<Contact> Contacts
    {
      get { return AddressBook; }
    }

    public Contact LoadContact(string id)
    {
      return AddressBook.Load(id);
    }

    public bool LoadSupported
    {
      get { return false; }
    }

    public bool PreferContactAggregation
    {
      get;
      set;
    }

    public bool AggregateContactsSupported
    {
      get { return true; }
    }

    public bool SingleContactsSupported
    {
      get { return false; }
    }

    public bool IsReadOnly
    {
      get { return true; }
    }
  }
}