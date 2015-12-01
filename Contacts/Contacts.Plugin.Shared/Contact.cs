/*using Plugin.Contacts.Abstractions;

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

namespace Plugin.Contacts.Abstractions
{
  public partial class Contact
  {

   

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
        if (person == null || !person.HasImage)
				  return null;

			  IntPtr data;
			  lock (person)
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
        return null;
#else
        return null;
#endif
      }
    }

  }
}
*/
