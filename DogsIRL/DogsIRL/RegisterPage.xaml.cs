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

        //    public async Task RegisterUserAsync(
        //string email, string password, string confirmPassword, string username)
        //    {
        //        var model = new RegisterInput
        //        {
        //            Username = username,
        //            Email = email,
        //            Password = password,
        //            ConfirmPassword = confirmPassword
        //        };
        //        var json = JsonConvert.SerializeObject(model);
        //        HttpContent httpContent = new StringContent(json);
        //        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var client = new HttpClient();
        //        var response = await client.PostAsync(
        //            "http://localhost:49563/api/Account/Register", httpContent);
        //    }

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
                await Navigation.PushAsync(new ProfileView());
            }
        }
    }
}
