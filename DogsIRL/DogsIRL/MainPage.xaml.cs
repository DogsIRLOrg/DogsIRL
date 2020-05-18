using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DogsIRL
{
    
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void SignInButtonOnClicked(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new ProfileView());
        }

        async void RegisterButtonOnClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Register());
        }
    }
}
