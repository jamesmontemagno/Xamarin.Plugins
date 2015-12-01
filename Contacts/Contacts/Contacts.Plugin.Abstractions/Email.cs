using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Contacts.Abstractions
{
  public enum EmailType
  {
    Home,
    Work,
    Other
  }

  public class Email
  {
    public EmailType Type
    {
      get;
      set;
    }

    public string Label
    {
      get;
      set;
    }

    public string Address
    {
      get;
      set;
    }
  }
}
