
using Xamarin.Forms;

namespace TestAppForms
{
    public class App2 : Application
    {
        public App2()
        {
            // The root page of your application
            MainPage = App.GetMainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
    public class App
    {
        public static Page GetMainPage()
        {

            return new NavigationPage(new Home())
            {
                BarBackgroundColor = Color.FromHex("2B84D3"),
                BarTextColor = Color.White
            };
        }
    }
}
