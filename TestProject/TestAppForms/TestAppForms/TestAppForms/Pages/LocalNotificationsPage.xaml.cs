using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace TestAppForms.Pages
{
    public partial class LocalNotificationsPage : ContentPage
    {
        public LocalNotificationsPage()
        {
            InitializeComponent();

            ButtonNotification.Clicked += (sender, args) =>
            {
                CrossLocalNotifications.Current.Show("Hello", "World!");
            };

            ButtonSchedule.Clicked += async (sender, e) => 
                {
                    CrossLocalNotifications.Current.Show("Hello", "from the future", 1, DateTime.Now.AddMinutes(1));
                    await DisplayAlert("Scheduled", "Form 1 minute in the future", "OK");
                };
        }
    }
}
