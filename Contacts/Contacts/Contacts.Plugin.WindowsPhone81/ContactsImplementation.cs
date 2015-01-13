using Contacts.Plugin.Abstractions;
using System;


namespace Contacts.Plugin
{
  /// <summary>
  /// Implementation for Contacts
  /// </summary>
  public class ContactsImplementation : IContacts
  {
    public System.Threading.Tasks.Task<bool> RequestPermission()
    {
      throw new NotImplementedException();
    }

    public System.Linq.IQueryable<Abstractions.Contact> Contacts
    {
      get { throw new NotImplementedException(); }
    }

    public Abstractions.Contact LoadContact(string id)
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