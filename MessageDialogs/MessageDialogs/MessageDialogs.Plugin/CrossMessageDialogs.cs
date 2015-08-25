using MessageDialogs.Plugin.Abstractions;
using System;

namespace MessageDialogs.Plugin
{
  /// <summary>
  /// Cross platform MessageDialogs implemenations
  /// </summary>
  public class CrossMessageDialogs
  {
    static Lazy<IMessageDialogs> Implementation = new Lazy<IMessageDialogs>(() => CreateMessageDialogs(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IMessageDialogs Current
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

    static IMessageDialogs CreateMessageDialogs()
    {
#if PORTABLE
        return null;
#else
        return new MessageDialogsImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
