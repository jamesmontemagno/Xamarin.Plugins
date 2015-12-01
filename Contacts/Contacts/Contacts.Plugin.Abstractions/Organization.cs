using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Contacts.Abstractions
{
  public enum OrganizationType
  {
    Work,
    Other
  }

  public class Organization
  {
    public OrganizationType Type
    {
      get;
      set;
    }

    public string Label
    {
      get;
      set;
    }

    public string Name
    {
      get;
      set;
    }

    public string ContactTitle
    {
      get;
      set;
    }
  }
}
