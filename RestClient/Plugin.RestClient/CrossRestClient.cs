using Plugin.RestClient.Abstractions;
using System;

namespace Plugin.RestClient
{
  /// <summary>
  /// Cross platform RestClient implemenations
  /// </summary>
  public class CrossRestClient
  {
    static Lazy<IRestClient> Implementation = new Lazy<IRestClient>(() => CreateRestClient(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IRestClient Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static IRestClient CreateRestClient()
    {
#if PORTABLE
        return null;
#else
        return new RestClientImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
