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
        

        async void CashAppOnClicked(System.Object sender, System.EventArgs e)
        {
            await Browser.OpenAsync("https://cash.app/$AndrewC921", BrowserLaunchMode.SystemPreferred);
         
        }

        async void VenmoOnClicked(System.Object sender, System.EventArgs e)
        {
            await Browser.OpenAsync("https://www.venmo.com/andrew-casper", BrowserLaunchMode.SystemPreferred);

        }
    }
}
