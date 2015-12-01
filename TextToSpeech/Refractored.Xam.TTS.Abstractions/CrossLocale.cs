using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.TextToSpeech.Abstractions
{
  /// <summary>
  /// Struct for a language local
  /// </summary>
  public struct CrossLocale
  {
    /// <summary>
    /// Main language code for iOS/WP/Android
    /// </summary>
    public string Language { get; set; }
    /// <summary>
    /// Country code to use on Android
    /// </summary>
    public string Country { get; set; }
    /// <summary>
    /// Friendy Display Name if avaialble
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Language + Country if avaialble
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return Language +
        (string.IsNullOrWhiteSpace(Country) ? string.Empty : "-" + Country);
    }
  }
}
