using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Messaging;

namespace TestAppForms.Pages
{
    public partial class MessagingPage : ContentPage
    {
        public MessagingPage()
        {
            InitializeComponent();
            buttonCall.Clicked += async (sender, e) => 
                {
                    try
                    {
                        // Make Phone Call
                        var phoneCallTask = MessagingPlugin.PhoneDialer;
                        if (phoneCallTask.CanMakePhoneCall) 
                            phoneCallTask.MakePhoneCall(phone.Text);
                        else
                            await DisplayAlert("Error", "This device can't place calls", "OK");
                    }
                    catch
                    {
                        await DisplayAlert("Error", "Unable to perform action", "OK");
                    }
                };

            buttonSms.Clicked += async (sender, e) => 
                {
                    try
                    {
                        
                        var smsTask = MessagingPlugin.SmsMessenger;
                        if (smsTask.CanSendSms) 
                            smsTask.SendSms(phone.Text, "Hello World");
                        else
                            await DisplayAlert("Error", "This device can't send sms", "OK");
                    }
                    catch
                    {
                        await DisplayAlert("Error", "Unable to perform action", "OK");
                    }
                };

            buttonEmail.Clicked += async (sender, e) => 
                {
                    try
                    {
                        var emailTask = MessagingPlugin.EmailMessenger;
                        if (emailTask.CanSendEmail) 
                            emailTask.SendEmail(email.Text, "Hello there!", "This was sent from the Xamrain Messaging Plugin from shared code!");
                        else
                            await DisplayAlert("Error", "This device can't send emails", "OK");
                    }
                    catch
                    {
                        await DisplayAlert("Error", "Unable to perform action", "OK");
                    }
                };
        }
    }
}

