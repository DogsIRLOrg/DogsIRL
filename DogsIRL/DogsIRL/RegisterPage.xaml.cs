using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DogsIRL.Models;
using DogsIRL.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class RegisterPage : ContentPage
    {
        private readonly ApiAccountService _apiAccountService;

        public RegisterPage()
        {
            InitializeComponent();
            _apiAccountService = new ApiAccountService();
        }

        /// <summary>
        /// This method is wired to register button in the front. It will grab user input and create an account. It will then send a request to backend API.
        /// Backend API will response back with a token. We also set global username as current username.
        /// </summary>
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
                    $"{App.ApiUrl}/account/register", httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Registration failed", "Oh no! We weren't able to register. Make sure your username, email, and password are valid, or try again later.", "Return");
                }
                App.Username = model.Username;
                string token = await _apiAccountService.RequestJwtTokenFromApi();
                App.Token = token;
                await Navigation.PushAsync(new ProfileView());
            }
        }
    }
}
