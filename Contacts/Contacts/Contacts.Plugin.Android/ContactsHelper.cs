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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Provider;

using StructuredName = Android.Provider.ContactsContract.CommonDataKinds.StructuredName;
using StructuredPostal = Android.Provider.ContactsContract.CommonDataKinds.StructuredPostal;
using CommonColumns = Android.Provider.ContactsContract.CommonDataKinds.CommonColumns;
using Uri = Android.Net.Uri;
using InstantMessaging = Android.Provider.ContactsContract.CommonDataKinds.Im;
using OrganizationData = Android.Provider.ContactsContract.CommonDataKinds.Organization;
using WebsiteData = Android.Provider.ContactsContract.CommonDataKinds.Website;
using Relation = Android.Provider.ContactsContract.CommonDataKinds.Relation;
using Contacts.Plugin.Abstractions;
using System.Threading.Tasks;

namespace Contacts.Plugin
{
  internal static class ContactHelper
  {
    internal static IEnumerable<Contact> GetContacts(bool rawContacts, ContentResolver content, Resources resources)
    {
      Uri curi = (rawContacts)
            ? ContactsContract.RawContacts.ContentUri
            : ContactsContract.Contacts.ContentUri;

      ICursor cursor = null;
      try
      {
        cursor = content.Query(curi, null, null, null, null);
        if (cursor == null)
          yield break;
		foreach (Contact contact in GetContacts(cursor, rawContacts, content, resources, 256))
          yield return contact;
      }
      finally
      {
        if (cursor != null)
          cursor.Close();
      }
    }

    internal static IEnumerable<Contact> GetContacts(ICursor cursor, bool rawContacts, ContentResolver content, Resources resources, int batchSize)
    {
      if (cursor == null)
        yield break;

      string column = (rawContacts)
                ? ContactsContract.RawContactsColumns.ContactId
                : ContactsContract.ContactsColumns.LookupKey;

      string[] ids = new string[batchSize];
      int columnIndex = cursor.GetColumnIndex(column);

      HashSet<string> uniques = new HashSet<string>();

      int i = 0;
      while (cursor.MoveToNext())
      {
        if (i == batchSize)
        {
          i = 0;
          foreach (Contact c in GetContacts(rawContacts, content, resources, ids))
            yield return c;
        }

        string id = cursor.GetString(columnIndex);
        if (uniques.Contains(id))
          continue;

        uniques.Add(id);
        ids[i++] = id;
      }

      if (i > 0)
      {
        foreach (Contact c in GetContacts(rawContacts, content, resources, ids.Take(i).ToArray()))
          yield return c;
      }
    }

    internal static IEnumerable<Contact> GetContacts(bool rawContacts, ContentResolver content, Resources resources, string[] ids)
    {
      ICursor c = null;

      string column = (rawContacts)
                ? ContactsContract.RawContactsColumns.ContactId
                : ContactsContract.ContactsColumns.LookupKey;

      StringBuilder whereb = new StringBuilder();
      for (int i = 0; i < ids.Length; i++)
      {
        if (i > 0)
          whereb.Append(" OR ");

        whereb.Append(column);
        whereb.Append("=?");
      }

      int x = 0;
      var map = new Dictionary<string, Contact>(ids.Length);

      try
      {
        Contact currentContact = null;

        c = content.Query(ContactsContract.Data.ContentUri, null, whereb.ToString(), ids, ContactsContract.ContactsColumns.LookupKey);
        if (c == null)
          yield break;

        int idIndex = c.GetColumnIndex(column);
        int dnIndex = c.GetColumnIndex(ContactsContract.ContactsColumns.DisplayName);
        while (c.MoveToNext())
        {
          string id = c.GetString(idIndex);
          if (currentContact == null || currentContact.Id != id)
          {
            // We need to yield these in the original ID order
            if (currentContact != null)
            {
              if (currentContact.Id == ids[x])
              {
                yield return currentContact;
                x++;
              }
              else
                map.Add(currentContact.Id, currentContact);
            }

            currentContact = new Contact(id, !rawContacts)
            {
              Tag = content
            };
            currentContact.DisplayName = c.GetString(dnIndex);
          }

          FillContactWithRow(resources, currentContact, c);
        }

        if (currentContact != null)
          map.Add(currentContact.Id, currentContact);

        for (; x < ids.Length; x++)
        {
            Contact tContact = null;
            if(map.TryGetValue(ids[x], out tContact))
                yield return tContact;
        }
      }
      finally
      {
        if (c != null)
          c.Close();
      }
    }

    internal static Contact GetContact(bool rawContact, ContentResolver content, Resources resources, ICursor cursor)
    {
      string id = (rawContact)
              ? cursor.GetString(cursor.GetColumnIndex(ContactsContract.RawContactsColumns.ContactId))
              : cursor.GetString(cursor.GetColumnIndex(ContactsContract.ContactsColumns.LookupKey));

      var contact = new Contact(id, !rawContact)
      {
        Tag = content
      };
      contact.DisplayName = GetString(cursor, ContactsContract.ContactsColumns.DisplayName);

      FillContactExtras(rawContact, content, resources, id, contact);

      return contact;
    }

    internal static void FillContactExtras(bool rawContact, ContentResolver content, Resources resources, string recordId, Contact contact)
    {
      ICursor c = null;

      string column = (rawContact)
                ? ContactsContract.RawContactsColumns.ContactId
                : ContactsContract.ContactsColumns.LookupKey;

      try
      {
		if (string.IsNullOrWhiteSpace(recordId))
			return;

        c = content.Query(ContactsContract.Data.ContentUri, null, column + " = ?", new[] { recordId }, null);
        if (c == null)
          return;

        while (c.MoveToNext())
          FillContactWithRow(resources, contact, c);
      }
      finally
      {
        if (c != null)
          c.Close();
      }
    }

    private static void FillContactWithRow(Resources resources, Contact contact, ICursor c)
    {
      string dataType = c.GetString(c.GetColumnIndex(ContactsContract.DataColumns.Mimetype));
      switch (dataType)
      {
        case ContactsContract.CommonDataKinds.Nickname.ContentItemType:
          contact.Nickname = c.GetString(c.GetColumnIndex(ContactsContract.CommonDataKinds.Nickname.Name));
          break;

        case StructuredName.ContentItemType:
          contact.Prefix = c.GetString(StructuredName.Prefix);
          contact.FirstName = c.GetString(StructuredName.GivenName);
          contact.MiddleName = c.GetString(StructuredName.MiddleName);
          contact.LastName = c.GetString(StructuredName.FamilyName);
          contact.Suffix = c.GetString(StructuredName.Suffix);
          break;

        case ContactsContract.CommonDataKinds.Phone.ContentItemType:
          contact.Phones.Add(GetPhone(c, resources));
          break;

        case ContactsContract.CommonDataKinds.Email.ContentItemType:
          contact.Emails.Add(GetEmail(c, resources));
          break;

        case ContactsContract.CommonDataKinds.Note.ContentItemType:
          contact.Notes.Add(GetNote(c, resources));
          break;

        case ContactsContract.CommonDataKinds.Organization.ContentItemType:
          contact.Organizations.Add(GetOrganization(c, resources));
          break;

        case StructuredPostal.ContentItemType:
          contact.Addresses.Add(GetAddress(c, resources));
          break;

        case InstantMessaging.ContentItemType:
          contact.InstantMessagingAccounts.Add(GetImAccount(c, resources));
          break;

        case WebsiteData.ContentItemType:
          contact.Websites.Add(GetWebsite(c, resources));
          break;

        case Relation.ContentItemType:
          contact.Relationships.Add(GetRelationship(c, resources));
          break;
      }
    }

    internal static Note GetNote(ICursor c, Resources resources)
    {
      return new Note { Contents = GetString(c, ContactsContract.DataColumns.Data1) };
    }

    internal static Relationship GetRelationship(ICursor c, Resources resources)
    {
      Relationship r = new Relationship { Name = c.GetString(Relation.Name) };

      RelationDataKind rtype = (RelationDataKind)c.GetInt(c.GetColumnIndex(CommonColumns.Type));
      switch (rtype)
      {
        case RelationDataKind.DomesticPartner:
        case RelationDataKind.Spouse:
        case RelationDataKind.Friend:
          r.Type = RelationshipType.SignificantOther;
          break;

        case RelationDataKind.Child:
          r.Type = RelationshipType.Child;
          break;

        default:
          r.Type = RelationshipType.Other;
          break;
      }

      return r;
    }

    internal static InstantMessagingAccount GetImAccount(ICursor c, Resources resources)
    {
      InstantMessagingAccount ima = new InstantMessagingAccount();
      ima.Account = c.GetString(CommonColumns.Data);

      //IMTypeDataKind imKind = (IMTypeDataKind) c.GetInt (c.GetColumnIndex (CommonColumns.Type));
      //ima.Type = imKind.ToInstantMessagingType();
      //ima.Label = InstantMessaging.GetTypeLabel (resources, imKind, c.GetString (CommonColumns.Label));

      IMProtocolDataKind serviceKind = (IMProtocolDataKind)c.GetInt(c.GetColumnIndex(InstantMessaging.Protocol));
      ima.Service = serviceKind.ToInstantMessagingService();
      ima.ServiceLabel = (serviceKind != IMProtocolDataKind.Custom)
                ? InstantMessaging.GetProtocolLabel(resources, serviceKind, String.Empty)
                : c.GetString(InstantMessaging.CustomProtocol);

      return ima;
    }

    internal static Address GetAddress(ICursor c, Resources resources)
    {
      Address a = new Address();
      a.Country = c.GetString(StructuredPostal.Country);
      a.Region = c.GetString(StructuredPostal.Region);
      a.City = c.GetString(StructuredPostal.City);
      a.PostalCode = c.GetString(StructuredPostal.Postcode);

      AddressDataKind kind = (AddressDataKind)c.GetInt(c.GetColumnIndex(CommonColumns.Type));
      a.Type = kind.ToAddressType();
      a.Label = (kind != AddressDataKind.Custom)
            ? StructuredPostal.GetTypeLabel(resources, kind, String.Empty)
            : c.GetString(CommonColumns.Label);

      string street = c.GetString(StructuredPostal.Street);
      string pobox = c.GetString(StructuredPostal.Pobox);
      if (street != null)
        a.StreetAddress = street;
      if (pobox != null)
      {
        if (street != null)
          a.StreetAddress += Environment.NewLine;

        a.StreetAddress += pobox;
      }
      return a;
    }

    internal static Phone GetPhone(ICursor c, Resources resources)
    {
      Phone p = new Phone();
      p.Number = GetString(c, ContactsContract.CommonDataKinds.Phone.Number);

      PhoneDataKind pkind = (PhoneDataKind)c.GetInt(c.GetColumnIndex(CommonColumns.Type));
      p.Type = pkind.ToPhoneType();
      p.Label = (pkind != PhoneDataKind.Custom)
            ? ContactsContract.CommonDataKinds.Phone.GetTypeLabel(resources, pkind, String.Empty)
            : c.GetString(CommonColumns.Label);

      return p;
    }

    internal static Email GetEmail(ICursor c, Resources resources)
    {
      Email e = new Email();
      e.Address = c.GetString(ContactsContract.DataColumns.Data1);

      EmailDataKind ekind = (EmailDataKind)c.GetInt(c.GetColumnIndex(CommonColumns.Type));
      e.Type = ekind.ToEmailType();
      e.Label = (ekind != EmailDataKind.Custom)
            ? ContactsContract.CommonDataKinds.Email.GetTypeLabel(resources, ekind, String.Empty)
            : c.GetString(CommonColumns.Label);

      return e;
    }

    internal static Organization GetOrganization(ICursor c, Resources resources)
    {
      Organization o = new Organization();
      o.Name = c.GetString(OrganizationData.Company);
      o.ContactTitle = c.GetString(OrganizationData.Title);

      OrganizationDataKind d = (OrganizationDataKind)c.GetInt(c.GetColumnIndex(CommonColumns.Type));
      o.Type = d.ToOrganizationType();
      o.Label = (d != OrganizationDataKind.Custom)
            ? OrganizationData.GetTypeLabel(resources, d, String.Empty)
            : c.GetString(CommonColumns.Label);

      return o;
    }

    internal static Website GetWebsite(ICursor c, Resources resources)
    {
      Website w = new Website();
      w.Address = c.GetString(WebsiteData.Url);

      //WebsiteDataKind kind = (WebsiteDataKind)c.GetInt (c.GetColumnIndex (CommonColumns.Type));
      //w.Type = kind.ToWebsiteType();
      //w.Label = (kind != WebsiteDataKind.Custom)
      //            ? resources.GetString ((int) kind)
      //            : c.GetString (CommonColumns.Label);

      return w;
    }

    //internal static WebsiteType ToWebsiteType (this WebsiteDataKind websiteKind)
    //{
    //    switch (websiteKind)
    //    {
    //        case WebsiteDataKind.Work:
    //            return WebsiteType.Work;
    //        case WebsiteDataKind.Home:
    //            return WebsiteType.Home;

    //        default:
    //            return WebsiteType.Other;
    //    }
    //}

    internal static string GetString(this ICursor c, string colName)
    {
      return c.GetString(c.GetColumnIndex(colName));
    }

    internal static AddressType ToAddressType(this AddressDataKind addressKind)
    {
      switch (addressKind)
      {
        case AddressDataKind.Home:
          return AddressType.Home;
        case AddressDataKind.Work:
          return AddressType.Work;
        default:
          return AddressType.Other;
      }
    }

    internal static EmailType ToEmailType(this EmailDataKind emailKind)
    {
      switch (emailKind)
      {
        case EmailDataKind.Home:
          return EmailType.Home;
        case EmailDataKind.Work:
          return EmailType.Work;
        default:
          return EmailType.Other;
      }
    }

    internal static PhoneType ToPhoneType(this PhoneDataKind phoneKind)
    {
      switch (phoneKind)
      {
        case PhoneDataKind.Home:
          return PhoneType.Home;
        case PhoneDataKind.Mobile:
          return PhoneType.Mobile;
        case PhoneDataKind.FaxHome:
          return PhoneType.HomeFax;
        case PhoneDataKind.Work:
          return PhoneType.Work;
        case PhoneDataKind.FaxWork:
          return PhoneType.WorkFax;
        case PhoneDataKind.Pager:
        case PhoneDataKind.WorkPager:
          return PhoneType.Pager;
        default:
          return PhoneType.Other;
      }
    }

    internal static OrganizationType ToOrganizationType(this OrganizationDataKind organizationKind)
    {
      switch (organizationKind)
      {
        case OrganizationDataKind.Work:
          return OrganizationType.Work;

        default:
          return OrganizationType.Other;
      }
    }

    internal static InstantMessagingService ToInstantMessagingService(this IMProtocolDataKind protocolKind)
    {
      switch (protocolKind)
      {
        case IMProtocolDataKind.Aim:
          return InstantMessagingService.Aim;
        case IMProtocolDataKind.Msn:
          return InstantMessagingService.Msn;
        case IMProtocolDataKind.Yahoo:
          return InstantMessagingService.Yahoo;
        case IMProtocolDataKind.Jabber:
          return InstantMessagingService.Jabber;
        case IMProtocolDataKind.Icq:
          return InstantMessagingService.Icq;
        default:
          return InstantMessagingService.Other;
      }
    }

    //internal static InstantMessagingType ToInstantMessagingType (this IMTypeDataKind imKind)
    //{
    //    switch (imKind)
    //    {
    //        case IMTypeDataKind.Home:
    //            return InstantMessagingType.Home;
    //        case IMTypeDataKind.Work:
    //            return InstantMessagingType.Work;
    //        default:
    //            return InstantMessagingType.Other;
    //    }
    //}
  }
}