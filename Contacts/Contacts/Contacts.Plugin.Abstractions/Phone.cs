using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Contacts.Abstractions
{

  public enum PhoneType
  {
    Home,
    HomeFax,
    Work,
    WorkFax,
    Pager,
    Mobile,
    Other,
  }

  public class Phone
  {
    public PhoneType Type
    {
      get;
      set;
    }

    public string Label
    {
      get;
      set;
    }

    public string Number
    {
      get;
      set;
    }
  }
}
