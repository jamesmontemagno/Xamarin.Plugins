using Contacts.Plugin.Abstractions;
using System;
using System.Linq;
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
      throw new NotImplementedException();
    }

    public IQueryable<Contact> Contacts
    {
      get { throw new NotImplementedException(); }
    }

    public Contact LoadContact(string id)
    {
      throw new NotImplementedException();
    }

    public bool LoadSupported
    {
      get { throw new NotImplementedException(); }
    }

    public bool PreferContactAggregation
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool AggregateContactsSupported
    {
      get { throw new NotImplementedException(); }
    }

    public bool SingleContactsSupported
    {
      get { throw new NotImplementedException(); }
    }

    public bool IsReadOnly
    {
      get { throw new NotImplementedException(); }
    }
  }
}