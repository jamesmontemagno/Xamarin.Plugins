using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refractored.Xam.TTS.Abstractions
{
  public struct CrossLocale
  {
    public string Language { get; set; }
    public string Country { get; set; }
    public string DisplayName { get; set; }

    public override string ToString()
    {
      return Language +
        (string.IsNullOrWhiteSpace(Country) ? string.Empty : "-" + Country);
    }
  }
}
