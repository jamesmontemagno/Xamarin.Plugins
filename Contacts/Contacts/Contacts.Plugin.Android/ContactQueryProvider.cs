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
using Android.Content;
using Android.Content.Res;
using Plugin.Contacts.Abstractions;

namespace Plugin.Contacts
{
  internal class ContactQueryProvider
    : ContentQueryProvider
  {
    internal ContactQueryProvider(ContentResolver content, Resources resources)
      : base(content, resources, new ContactTableFinder())
    {
    }

    public bool UseRawContacts
    {
      get { return ((ContactTableFinder)TableFinder).UseRawContacts; }
      set { ((ContactTableFinder)TableFinder).UseRawContacts = value; }
    }

    protected override IEnumerable GetObjectReader(ContentQueryTranslator translator)
    {
      if (translator == null || translator.ReturnType == null || translator.ReturnType == typeof(Contact))
        return new ContactReader(UseRawContacts, translator, content, resources);
      else if (translator.ReturnType == typeof(Phone))
        return new GenericQueryReader<Phone>(translator, content, resources, ContactHelper.GetPhone);
      else if (translator.ReturnType == typeof(Email))
        return new GenericQueryReader<Email>(translator, content, resources, ContactHelper.GetEmail);
      else if (translator.ReturnType == typeof(Address))
        return new GenericQueryReader<Address>(translator, content, resources, ContactHelper.GetAddress);
      else if (translator.ReturnType == typeof(Relationship))
        return new GenericQueryReader<Relationship>(translator, content, resources, ContactHelper.GetRelationship);
      else if (translator.ReturnType == typeof(InstantMessagingAccount))
        return new GenericQueryReader<InstantMessagingAccount>(translator, content, resources, ContactHelper.GetImAccount);
      else if (translator.ReturnType == typeof(Website))
        return new GenericQueryReader<Website>(translator, content, resources, ContactHelper.GetWebsite);
      else if (translator.ReturnType == typeof(Organization))
        return new GenericQueryReader<Organization>(translator, content, resources, ContactHelper.GetOrganization);
      else if (translator.ReturnType == typeof(Note))
        return new GenericQueryReader<Note>(translator, content, resources, ContactHelper.GetNote);
      else if (translator.ReturnType == typeof(string))
        return new ProjectionReader<string>(content, translator, (cur, col) => cur.GetString(col));
      else if (translator.ReturnType == typeof(int))
        return new ProjectionReader<int>(content, translator, (cur, col) => cur.GetInt(col));

      throw new ArgumentException();
    }
  }
}