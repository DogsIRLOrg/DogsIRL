using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class DonatePage : ContentPage
    {
        public DonatePage()
        {
            InitializeComponent();
        }
        

        async void PaypalOnClicked(System.Object sender, System.EventArgs e)
        {
            await Browser.OpenAsync("https://www.paypal.com/", BrowserLaunchMode.SystemPreferred);
         
        }

        async void VenmoOnClicked(System.Object sender, System.EventArgs e)
        {
            await Browser.OpenAsync("https://www.google.com/maps", BrowserLaunchMode.SystemPreferred);

        }
    }
}
