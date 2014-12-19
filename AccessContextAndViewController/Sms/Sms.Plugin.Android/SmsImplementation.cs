using Android.Content;
using Sms.Plugin.Abstractions;
using System;


namespace Sms.Plugin
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class SmsImplementation : ISms
  {
    public void SendSms(string body, string phoneNumber)
    {
      var smsUri = Android.Net.Uri.Parse("smsto:" + phoneNumber);
      var smsIntent = new Intent(Intent.ActionSendto, smsUri);
      smsIntent.PutExtra("sms_body", body);
      smsIntent.PutExtra(Intent.ExtraText, body);

      //these flags are required when using application context
      smsIntent.SetFlags(ActivityFlags.ClearTop);
      smsIntent.SetFlags(ActivityFlags.NewTask);
      Android.App.Application.Context.StartActivity(smsIntent);
    }
  }
}