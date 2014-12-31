using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Plugin.Abstractions
{
  public enum AddressType
  {
    Work,
    Home,
    Other
  }

  public class Address
  {
    public AddressType Type
    {
      get;
      set;
    }

    public string Label
    {
      get;
      set;
    }

    public string StreetAddress
    {
      get;
      set;
    }

    public string City
    {
      get;
      set;
    }

    public string Region
    {
      get;
      set;
    }

    public string Country
    {
      get;
      set;
    }

    public string PostalCode
    {
      get;
      set;
    }
  }
}
