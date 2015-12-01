using Plugin.Contacts.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Plugin.Contacts
{
    /// <summary>
    /// Implementation for Contacts
    /// </summary>
    public class ContactsImplementation : IContacts
    {
        public Task<bool> RequestPermission()
        {
            return Task.FromResult(false);
        }

        public IQueryable<Contact> Contacts
        {
            get { return null; }
        }

        public Contact LoadContact(string id)
        {
            return null;
        }

        public bool LoadSupported
        {
            get { return false; }
        }

        public bool PreferContactAggregation
        {
            get; set;
        }

        public bool AggregateContactsSupported
        {
            get { return false; }
        }

        public bool SingleContactsSupported
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}