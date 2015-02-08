//
//  Copyright 2011-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Provider;
using Contacts.Plugin.Abstractions;

namespace Contacts.Plugin
{
  public sealed class AddressBook
    : IQueryable<Task<Contact>>
  {
    public AddressBook(Context context)
    {
      if (context == null)
        throw new ArgumentNullException("context");

      this.content = context.ContentResolver;
      this.resources = context.Resources;
      this.contactsProvider = new ContactQueryProvider(context.ContentResolver, context.Resources);
    }


    public bool PreferContactAggregation
    {
      get { return !this.contactsProvider.UseRawContacts; }
      set { this.contactsProvider.UseRawContacts = !value; }
    }


    public IEnumerator<Task<Contact>> GetEnumerator()
    {
      return ContactHelper.GetContacts(!PreferContactAggregation, this.content, this.resources).GetEnumerator();
    }

    /// <summary>
    /// Attempts to load a contact for the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The <see cref="Contact"/> if found, <c>null</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is empty.</exception>
    public Contact Load(string id)
    {
      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("Invalid ID", "id");

      Android.Net.Uri curi; string column;
      if (PreferContactAggregation)
      {
        curi = ContactsContract.Contacts.ContentUri;
        column = ContactsContract.ContactsColumns.LookupKey;
      }
      else
      {
        curi = ContactsContract.RawContacts.ContentUri;
        column = ContactsContract.RawContactsColumns.ContactId;
      }

      ICursor c = null;
      try
      {
        c = this.content.Query(curi, null, column + " = ?", new[] { id }, null);
        return (c.MoveToNext() ? ContactHelper.GetContact(!PreferContactAggregation, this.content, this.resources, c) : null);
      }
      finally
      {
        if (c != null)
          c.Deactivate();
      }
    }

    //public Contact SaveNew (Contact contact)
    //{
    //    if (contact == null)
    //        throw new ArgumentNullException ("contact");
    //    if (contact.Id != null)
    //        throw new ArgumentException ("Contact is not new", "contact");

    //    throw new NotImplementedException();
    //}

    //public Contact SaveExisting (Contact contact)
    //{
    //    if (contact == null)
    //        throw new ArgumentNullException ("contact");
    //    if (String.IsNullOrWhiteSpace (contact.Id))
    //        throw new ArgumentException ("Contact is not existing");

    //    throw new NotImplementedException();

    //    return Load (contact.Id);
    //}

    //public Contact Save (Contact contact)
    //{
    //    if (contact == null)
    //        throw new ArgumentNullException ("contact");

    //    return (String.IsNullOrWhiteSpace (contact.Id) ? SaveNew (contact) : SaveExisting (contact));
    //}

    //public void Delete (Contact contact)
    //{
    //    if (contact == null)
    //        throw new ArgumentNullException ("contact");
    //    if (!String.IsNullOrWhiteSpace (contact.Id))
    //        throw new ArgumentException ("Contact is not a persisted instance", "contact");

    //    // TODO: Does this cascade?
    //    this.content.Delete (ContactsContract.RawContacts.ContentUri, ContactsContract.RawContactsColumns.ContactId + " = ?", new[] { contact.Id });
    //}

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    Type IQueryable.ElementType
    {
      get { return typeof(Contact); }
    }

    Expression IQueryable.Expression
    {
      get { return Expression.Constant(this); }
    }

    IQueryProvider IQueryable.Provider
    {
      get { return this.contactsProvider; }
    }

    private readonly ContactQueryProvider contactsProvider;
    private readonly ContentResolver content;
    private readonly Resources resources;
  }
}