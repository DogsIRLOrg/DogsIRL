using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DogsIRL.Models;
using Newtonsoft.Json;
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

        async void OnButtonClicked(System.Object sender, System.EventArgs e)
        {
            var signInUser = new UserSignIn()
            {
                UserName = NameEntry.Text,
                Password = Password.Text,
            };
            var json = JsonConvert.SerializeObject(signInUser);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(
                "https://localhost:5001/api/accounts/login", content);
            if(response.IsSuccessStatusCode)
            {
            await Navigation.PushAsync(new ProfileView());
            }
            await DisplayAlert("Failed Login Attempt", "Something has gone horribly wrong. Its probably your fault", "Try Again");
        }

        async void RegisterButtonOnClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProfileView());
        }
    }
}
