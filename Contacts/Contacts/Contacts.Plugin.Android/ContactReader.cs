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
using System.Text;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Provider;
using Plugin.Contacts.Abstractions;

namespace Plugin.Contacts
{
  internal class ContactReader
    : IEnumerable<Contact>
  {
    public ContactReader(bool useRawContacts, ContentQueryTranslator translator, ContentResolver content, Resources resources)
    {
      this.rawContacts = useRawContacts;
      this.translator = translator;
      this.content = content;
      this.resources = resources;
    }

    public IEnumerator<Contact> GetEnumerator()
    {
      Android.Net.Uri table = (this.rawContacts)
                    ? ContactsContract.RawContacts.ContentUri
                    : ContactsContract.Contacts.ContentUri;

      string query = null;
      string[] parameters = null;
      string sortString = null;
      string[] projections = null;

      if (this.translator != null)
      {
        table = this.translator.Table;
        query = this.translator.QueryString;
        parameters = this.translator.ClauseParameters;
        sortString = this.translator.SortString;

        if (this.translator.Projections != null)
        {
          projections = this.translator.Projections
                  .Where(p => p.Columns != null)
                  .SelectMany(t => t.Columns)
                  .ToArray();

          if (projections.Length == 0)
            projections = null;
        }

        if (this.translator.Skip > 0 || this.translator.Take > 0)
        {
          StringBuilder limitb = new StringBuilder();

          if (sortString == null)
            limitb.Append(ContactsContract.ContactsColumns.LookupKey);

          limitb.Append(" LIMIT ");

          if (this.translator.Skip > 0)
          {
            limitb.Append(this.translator.Skip);
            if (this.translator.Take > 0)
              limitb.Append(",");
          }

          if (this.translator.Take > 0)
            limitb.Append(this.translator.Take);

          sortString = (sortString == null) ? limitb.ToString() : sortString + limitb;
        }
      }

      ICursor cursor = null;
      try
      {
        cursor = this.content.Query(table, projections, query, parameters, sortString);
        if (cursor == null)
          yield break;

        foreach (Contact contact in ContactHelper.GetContacts(cursor, this.rawContacts, this.content, this.resources, BatchSize))
          yield return contact;
      }
      finally
      {
        if (cursor != null)
          cursor.Close();
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private readonly bool rawContacts;
    private readonly ContentQueryTranslator translator;
    private readonly ContentResolver content;
    private readonly Resources resources;

    private const int BatchSize = 20;
  }
}