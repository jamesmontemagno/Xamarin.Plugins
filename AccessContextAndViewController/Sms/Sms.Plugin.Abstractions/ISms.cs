using System;

namespace Sms.Plugin.Abstractions
{
  /// <summary>
  /// Interface for Sms
  /// </summary>
  public interface ISms
  {

    void SendSms(string body, string phoneNumber); 
  }
}
