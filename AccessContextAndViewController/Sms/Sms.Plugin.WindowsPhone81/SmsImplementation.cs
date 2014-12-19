using Sms.Plugin.Abstractions;
using System;


namespace Sms.Plugin
{
  /// <summary>
  /// Implementation for Sms
  /// </summary>
  public class SmsImplementation : ISms
  {
    public void SendSms(string body, string phoneNumber)
    {
      throw new NotImplementedException();
    }
  }
}