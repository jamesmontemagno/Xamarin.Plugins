using Sms.Plugin.Abstractions;
using System;

namespace Sms.Plugin
{
  /// <summary>
  /// Cross platform Sms implemenations
  /// </summary>
  public class CrossSms
  {
    static Lazy<ISms> Implementation = new Lazy<ISms>(() => CreateSms(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static ISms Current
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

    static ISms CreateSms()
    {
#if PORTABLE
        return null;
#else
        return new SmsImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
