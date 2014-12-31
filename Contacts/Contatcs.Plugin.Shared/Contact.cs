using Contacts.Plugin.Abstractions;

#if __ANDROID__
using Android.Provider;
using Android.Database;
#elif __IOS__

#if __UNIFIED__
using AddressBook;
using Foundation;
using UIKit;
#else
using MonoTouch.AddressBook;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

#elif WINDOWS_PHONE
#elif NETFX_CORE
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Plugin
{
  public class Contact
  {

    public Contact()
    {
    }

    public object Tag
    {
      get;
      set;
    }

    public Contact(string id, bool isAggregate)
    {
      IsAggregate = isAggregate;
      Id = id;
    }

    public string Id
    {
      get;
      private set;
    }

    public bool IsAggregate
    {
      get;
      private set;
    }

    public string DisplayName
    {
      get;
      set;
    }

    public string Prefix
    {
      get;
      set;
    }

    public string FirstName
    {
      get;
      set;
    }

    public string MiddleName
    {
      get;
      set;
    }

    public string LastName
    {
      get;
      set;
    }

    public string Nickname
    {
      get;
      set;
    }

    public string Suffix
    {
      get;
      set;
    }

    internal List<Relationship> relationships = new List<Relationship>();
    public List<Relationship> Relationships
    {
      get { return this.relationships; }
      set { this.relationships = new List<Relationship>(value); }
    }

    internal List<Address> addresses = new List<Address>();
    public List<Address> Addresses
    {
      get { return this.addresses; }
      set { this.addresses = new List<Address>(value); }
    }

    internal List<InstantMessagingAccount> instantMessagingAccounts = new List<InstantMessagingAccount>();
    public List<InstantMessagingAccount> InstantMessagingAccounts
    {
      get { return this.instantMessagingAccounts; }
      set { this.instantMessagingAccounts = new List<InstantMessagingAccount>(value); }
    }

    internal List<Website> websites = new List<Website>();
    public List<Website> Websites
    {
      get { return this.websites; }
      set { this.websites = new List<Website>(value); }
    }

    internal List<Organization> organizations = new List<Organization>();
    public List<Organization> Organizations
    {
      get { return this.organizations; }
      set { this.organizations = new List<Organization>(value); }
    }

    internal List<Note> notes = new List<Note>();
    public List<Note> Notes
    {
      get { return this.notes; }
      set { this.notes = new List<Note>(value); }
    }

    internal List<Email> emails = new List<Email>();
    public List<Email> Emails
    {
      get { return this.emails; }
      set { this.emails = new List<Email>(value); }
    }

    internal List<Phone> phones = new List<Phone>();
    public List<Phone> Phones
    {
      get { return this.phones; }
      set { this.phones = new List<Phone>(value); }
    }

#if __IOS__
    [DllImport("/System/Library/Frameworks/AddressBook.framework/AddressBook")]
    private static extern IntPtr ABPersonCopyImageDataWithFormat(IntPtr handle, ABPersonImageFormat format);
	
#endif

    public byte[] Thumbnail
    {
      get
      {
#if __ANDROID__
        string lookupColumn = (IsAggregate)
			                      	? ContactsContract.ContactsColumns.LookupKey
			                      	: ContactsContract.RawContactsColumns.ContactId;

			ICursor c = null;
			try {
				c = this.content.Query (ContactsContract.Data.ContentUri, new[] { ContactsContract.CommonDataKinds.Photo.PhotoColumnId, ContactsContract.DataColumns.Mimetype },
					lookupColumn + "=? AND " + ContactsContract.DataColumns.Mimetype + "=?", new[] { Id, ContactsContract.CommonDataKinds.Photo.ContentItemType }, null);

				while (c.MoveToNext()) {
					byte[] tdata = c.GetBlob (c.GetColumnIndex (ContactsContract.CommonDataKinds.Photo.PhotoColumnId));
					if (tdata != null)
						return tdata;
				}
			} finally {
				if (c != null)
					c.Close();
			}

			return null;
#elif __IOS__
        var person = Tag as ABPerson;
        if (person == null || !this.person.HasImage)
				  return null;

			  IntPtr data;
			  lock (this.person)
				  data = ABPersonCopyImageDataWithFormat (person.Handle, ABPersonImageFormat.Thumbnail);

			  if (data == IntPtr.Zero)
				  return null;

			  return new NSData (data).ToArray();
#elif WINDOWS_PHONE
      
        lock (this.Tag)
        {

          var contact = Tag as Microsoft.Phone.UserData.Contact;
          if (contact == null)
            return null;

          var s = contact.GetPicture();
          if (s == null)
            return null;

          return null;//read into byte[]
        }

#elif NETFX_CORE
#else
        return null;
#endif
      }
    }

  }
}
