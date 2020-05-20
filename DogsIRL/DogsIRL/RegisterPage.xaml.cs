using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

       // There used to be zombie code here


        async void OnButtonClicked(System.Object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(username.Text) && !string.IsNullOrWhiteSpace(email.Text))
            {
                var model = new RegisterInput
                {
                    Username = username.Text,
                    Email = email.Text,
                    Password = password.Text,
                    ConfirmPassword = confirmPassword.Text
                };
                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var client = new HttpClient();
                var response = await client.PostAsync(
                    "https://dogsirl-api.azurewebsites.net/api/account/register", httpContent);
                App.Username = model.Username;
                await Navigation.PushAsync(new ProfileView());
            }
        }
    }
}
