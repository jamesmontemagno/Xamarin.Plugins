using Plugin.Share;
using System;

using Xamarin.Forms;

namespace TestAppForms.Pages
{
    public partial class SharePage : ContentPage
    {
        public SharePage()
        {
            InitializeComponent();
        }

        void Button_OnClicked(object sender, EventArgs e)
        {
            switch(((Button)sender).StyleId)
            {
                case "Text":
                    CrossShare.Current.Share("Follow @JamesMontemagno on Twitter", "Share");
                    break;
                case "Link":
                    CrossShare.Current.ShareLink("http://motzcod.es", "Checkout my blog", "MotzCod.es");
                    break;
                case "Browser":
                    CrossShare.Current.OpenBrowser("http://motzcod.es");
                    break;
            }
        }
    }
}
