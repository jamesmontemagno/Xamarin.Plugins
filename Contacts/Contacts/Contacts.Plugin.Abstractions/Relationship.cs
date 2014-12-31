using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Plugin.Abstractions
{
  public enum RelationshipType
  {
    SignificantOther,
    Child,
    Other
  }

  public class Relationship
  {
    public string Name
    {
      get;
      set;
    }

    public RelationshipType Type
    {
      get;
      set;
    }
  }
}
