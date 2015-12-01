using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Contacts.Abstractions
{
  /// <summary>
  /// Interface for Contacts
  /// </summary>
  public interface IContacts
  {
    Task<bool> RequestPermission();
    IQueryable<Contact> Contacts { get; }
    Contact LoadContact(string id);

    bool LoadSupported { get; }
    bool PreferContactAggregation { get; set; }
    bool AggregateContactsSupported { get; }
    bool SingleContactsSupported { get; }
    bool IsReadOnly { get; }
  }
}
