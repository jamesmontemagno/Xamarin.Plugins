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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Phone.UserData;
using Expression = System.Linq.Expressions.Expression;
using WindowsContacts = Microsoft.Phone.UserData.Contacts;
using System.Threading;
using Contacts.Plugin.Abstractions;
using Contact = Contacts.Plugin.Abstractions.Contact;

namespace Contacts.Plugin
{
  internal class ContactQueryProvider
    : IQueryProvider
  {
    internal IEnumerable<Contact> GetContacts()
    {
      return new ContactSearch(null, FilterKind.None).Task.Result;
    }

    private class ContactSearch
    {
      internal ContactSearch(string filter, FilterKind filterKind)
      {
        System.Threading.Tasks.Task.Factory.StartNew(() =>
        {
          WindowsContacts contacts = new WindowsContacts();
          contacts.SearchCompleted += (s, e) => this.tcs.SetResult(e.Results.Select(GetContact));
          contacts.SearchAsync(filter, filterKind, null);
        }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
      }

      internal Task<IEnumerable<Contact>> Task
      {
        get { return this.tcs.Task; }
      }

      private readonly TaskCompletionSource<IEnumerable<Contact>> tcs = new TaskCompletionSource<IEnumerable<Contact>>();
    }

    IQueryable IQueryProvider.CreateQuery(Expression expression)
    {
      throw new NotImplementedException();
    }

    object IQueryProvider.Execute(Expression expression)
    {
      IQueryable<Contact> q = GetContacts().AsQueryable();

      expression = ReplaceQueryable(expression, q);

      if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(IQueryable<>))
        return q.Provider.CreateQuery(expression);
      else
        return q.Provider.Execute(expression);
    }

    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
    {
      return new Query<TElement>(this, expression);
    }

    TResult IQueryProvider.Execute<TResult>(Expression expression)
    {
      return (TResult)((IQueryProvider)this).Execute(expression);
    }

    private static Contact GetContact(Microsoft.Phone.UserData.Contact contact)
    {
      var c = new Contact
        {
          Tag = contact
        };
      c.DisplayName = contact.DisplayName;

      if (contact.CompleteName != null)
      {
        c.Prefix = contact.CompleteName.Title;
        c.FirstName = contact.CompleteName.FirstName;
        c.MiddleName = contact.CompleteName.MiddleName;
        c.LastName = contact.CompleteName.LastName;
        c.Suffix = contact.CompleteName.Suffix;
      }

      foreach (ContactAddress address in contact.Addresses)
        c.Addresses.Add(GetAddress(address));

      foreach (ContactEmailAddress email in contact.EmailAddresses)
        c.Emails.Add(GetEmail(email));

      foreach (ContactPhoneNumber phone in contact.PhoneNumbers)
        c.Phones.Add(GetPhone(phone));

      foreach (ContactCompanyInformation company in contact.Companies)
        c.Organizations.Add(GetOrganization(company));

      foreach (string name in contact.Children)
        c.Relationships.Add(new Relationship { Name = name, Type = RelationshipType.Child });

      foreach (string name in contact.SignificantOthers)
        c.Relationships.Add(new Relationship { Name = name, Type = RelationshipType.SignificantOther });

      foreach (string url in contact.Websites)
        c.Websites.Add(new Website { Address = url });

      foreach (string note in contact.Notes)
        c.Notes.Add(new Note { Contents = note });

      return c;
    }

    private static Organization GetOrganization(ContactCompanyInformation company)
    {
      Organization o = new Organization();
      o.Name = company.CompanyName;
      o.ContactTitle = company.JobTitle;
      o.Type = OrganizationType.Work;
      o.Label = "Work";

      return o;
    }

    private static Phone GetPhone(ContactPhoneNumber phone)
    {
      Phone p = new Phone();
      p.Number = phone.PhoneNumber;

      switch (phone.Kind)
      {
        case PhoneNumberKind.Home:
          p.Type = PhoneType.Home;
          p.Label = "Home";
          break;
        case PhoneNumberKind.HomeFax:
          p.Type = PhoneType.HomeFax;
          p.Label = "Home Fax";
          break;
        case PhoneNumberKind.Work:
          p.Type = PhoneType.Work;
          p.Label = "Work";
          break;
        case PhoneNumberKind.WorkFax:
          p.Type = PhoneType.WorkFax;
          p.Label = "Work Fax";
          break;
        case PhoneNumberKind.Mobile:
          p.Type = PhoneType.Mobile;
          p.Label = "Mobile";
          break;
        case PhoneNumberKind.Pager:
          p.Type = PhoneType.Pager;
          p.Label = "Pager";
          break;
        default:
          p.Type = PhoneType.Other;
          p.Label = "Other";
          break;
      }

      return p;
    }

    private static Email GetEmail(ContactEmailAddress email)
    {
      Email e = new Email();
      e.Address = email.EmailAddress;

      switch (email.Kind)
      {
        case EmailAddressKind.Work:
          e.Type = EmailType.Work;
          e.Label = "Work";
          break;
        case EmailAddressKind.Personal:
          e.Type = EmailType.Home;
          e.Label = "Home";
          break;
        default:
          e.Type = EmailType.Other;
          e.Label = "Other";
          break;
      }

      return e;
    }

    private static Address GetAddress(ContactAddress address)
    {
      var a = new Address();
      a.StreetAddress = address.PhysicalAddress.AddressLine1;
      if (!String.IsNullOrWhiteSpace(address.PhysicalAddress.AddressLine2))
        a.StreetAddress += Environment.NewLine + address.PhysicalAddress.AddressLine2;

      a.PostalCode = address.PhysicalAddress.PostalCode;
      a.City = address.PhysicalAddress.City;
      a.Region = address.PhysicalAddress.StateProvince;
      a.Country = address.PhysicalAddress.CountryRegion;

      switch (address.Kind)
      {
        case AddressKind.Home:
          a.Type = AddressType.Home;
          a.Label = "Home";
          break;
        case AddressKind.Work:
          a.Type = AddressType.Work;
          a.Label = "Work";
          break;
        default:
          a.Type = AddressType.Other;
          a.Label = "Other";
          break;
      }

      return a;
    }

    private Expression ReplaceQueryable(Expression expression, object value)
    {
      MethodCallExpression mc = expression as MethodCallExpression;
      if (mc != null)
      {
        Expression[] args = mc.Arguments.ToArray();
        Expression narg = ReplaceQueryable(mc.Arguments[0], value);
        if (narg != args[0])
        {
          args[0] = narg;
          return Expression.Call(mc.Method, args);
        }
        else
          return mc;
      }

      ConstantExpression c = expression as ConstantExpression;
      if (c != null && c.Type.GetInterfaces().Contains(typeof(IQueryable)))
        return Expression.Constant(value);

      return expression;
    }
  }
}