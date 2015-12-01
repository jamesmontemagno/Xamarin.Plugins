//
//  Copyright 2011-2014, Xamarin Inc.
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
using System.Linq.Expressions;
using System.Reflection;
using Android.Provider;
using Uri = Android.Net.Uri;
using Plugin.Contacts.Abstractions;

namespace Plugin.Contacts
{
  internal class ContactTableFinder
    : ExpressionVisitor, ITableFinder
  {
    public bool UseRawContacts
    {
      get;
      set;
    }

    public Uri DefaultTable
    {
      get { return (UseRawContacts) ? ContactsContract.RawContacts.ContentUri : ContactsContract.Contacts.ContentUri; }
    }

    public TableFindResult Find(Expression expression)
    {
      Visit(expression);

      var result = new TableFindResult(this.table, this.mimeType);

      this.table = null;
      this.mimeType = null;

      return result;
    }

    public bool IsSupportedType(Type type)
    {
      return type == typeof(Contact)
        || type == typeof(Phone)
        || type == typeof(Email)
        || type == typeof(Address)
        || type == typeof(Relationship)
        || type == typeof(InstantMessagingAccount)
        || type == typeof(Website)
        || type == typeof(Organization)
        || type == typeof(Note);
    }

    public ContentResolverColumnMapping GetColumn(MemberInfo member)
    {
      if (member.DeclaringType == typeof(Contact))
        return GetContactColumn(member);
      if (member.DeclaringType == typeof(Email))
        return GetEmailColumn(member);
      if (member.DeclaringType == typeof(Phone))
        return GetPhoneColumn(member);
      if (member.DeclaringType == typeof(Address))
        return GetAddressColumn(member);
      if (member.DeclaringType == typeof(Relationship))
        return GetRelationshipColumn(member);
      if (member.DeclaringType == typeof(InstantMessagingAccount))
        return GetImColumn(member);
      if (member.DeclaringType == typeof(Website))
        return GetWebsiteColumn(member);
      if (member.DeclaringType == typeof(Organization))
        return GetOrganizationColumn(member);
      if (member.DeclaringType == typeof(Note))
        return GetNoteColumn(member);

      return null;
    }

    private Uri table;
    private string mimeType;

    private ContentResolverColumnMapping GetNoteColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "Contents":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.CommonColumns.Data, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetOrganizationColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "ContactTitle":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.Organization.Title, typeof(string));
        case "Name":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.Organization.Company, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetWebsiteColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "Address":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.Website.Url, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetImColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "Account":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.CommonColumns.Data, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetRelationshipColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "Name":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.Relation.Name, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetAddressColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "City":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredPostal.City, typeof(string));
        case "Region":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredPostal.Region, typeof(string));
        case "Country":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredPostal.Country, typeof(string));
        case "PostalCode":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredPostal.Postcode, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetPhoneColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "Number":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.Phone.Number, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetEmailColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "Address":
          return new ContentResolverColumnMapping(ContactsContract.DataColumns.Data1, typeof(string));
      }

      return null;
    }

    private ContentResolverColumnMapping GetContactColumn(MemberInfo member)
    {
      switch (member.Name)
      {
        case "DisplayName":
          return new ContentResolverColumnMapping(ContactsContract.ContactsColumns.DisplayName, typeof(string));
        case "Prefix":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredName.Prefix, typeof(string));
        case "FirstName":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredName.GivenName, typeof(string));
        case "LastName":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredName.FamilyName, typeof(string));
        case "Suffix":
          return new ContentResolverColumnMapping(ContactsContract.CommonDataKinds.StructuredName.Suffix, typeof(string));

        case "Phones":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<Phone>));
        case "Emails":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<Email>));
        case "Addresses":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<Address>));
        case "Notes":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<Note>));
        case "Relationships":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<Relationship>));
        case "InstantMessagingAccounts":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<InstantMessagingAccount>));
        case "Websites":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<Website>));
        case "Organizations":
          return new ContentResolverColumnMapping((string)null, typeof(IEnumerable<Organization>));

        default:
          return null;
      }
    }


    protected override Expression VisitMemberAccess(MemberExpression node)
    {
      node = (MemberExpression)base.VisitMemberAccess(node);

      if (this.table == null)
      {
        if (node.Member.DeclaringType == typeof(Contact))
          this.table = GetContactTable(node);
        else if (node.Member.DeclaringType == typeof(Phone))
          this.table = ContactsContract.CommonDataKinds.Phone.ContentUri;
        else if (node.Member.DeclaringType == typeof(Email))
          this.table = ContactsContract.CommonDataKinds.Email.ContentUri;
      }

      return node;
    }

    private Uri GetContactTable(MemberExpression expression)
    {
      switch (expression.Member.Name)
      {
        case "DisplayName":
          return (UseRawContacts) ? ContactsContract.RawContacts.ContentUri : ContactsContract.Contacts.ContentUri;

        case "Prefix":
        case "FirstName":
        case "MiddleName":
        case "LastName":
        case "Suffix":
          this.mimeType = ContactsContract.CommonDataKinds.StructuredName.ContentItemType;
          return ContactsContract.Data.ContentUri;

        case "Relationships":
          this.mimeType = ContactsContract.CommonDataKinds.Relation.ContentItemType;
          return ContactsContract.Data.ContentUri;

        case "Organizations":
          this.mimeType = ContactsContract.CommonDataKinds.Organization.ContentItemType;
          return ContactsContract.Data.ContentUri;

        case "Notes":
          this.mimeType = ContactsContract.CommonDataKinds.Note.ContentItemType;
          return ContactsContract.Data.ContentUri;

        case "Phones":
          return ContactsContract.CommonDataKinds.Phone.ContentUri;
        case "Emails":
          return ContactsContract.CommonDataKinds.Email.ContentUri;
        case "Addresses":
          return ContactsContract.CommonDataKinds.StructuredPostal.ContentUri;

        case "Websites":
          this.mimeType = ContactsContract.CommonDataKinds.Website.ContentItemType;
          return ContactsContract.Data.ContentUri;

        case "InstantMessagingAccounts":
          this.mimeType = ContactsContract.CommonDataKinds.Im.ContentItemType;
          return ContactsContract.Data.ContentUri;

        default:
          return null;
      }
    }
  }
}