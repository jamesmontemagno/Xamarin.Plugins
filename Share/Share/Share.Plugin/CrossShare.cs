using Share.Plugin.Abstractions;
using System;

namespace Share.Plugin
{
  /// <summary>
  /// Cross platform Share implemenations
  /// </summary>
  public class CrossShare
  {
    static Lazy<IShare> Implementation = new Lazy<IShare>(() => CreateShare(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IShare Current
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

    static IShare CreateShare()
    {
#if PORTABLE
        return null;
#else
        return new ShareImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
