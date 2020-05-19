using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DogsIRL
{
    public partial class ParkPage : ContentPage
    {
        public ParkPage()
        {
            InitializeComponent();
        }

        async void OnInteractClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProfileView());
        }

        async void OnCollectClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProfileView());
        }
    }
}
