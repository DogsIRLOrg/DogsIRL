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

                var response = await _apiAccountService.RequestRegister(model);

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Registration failed", "Oh no! We weren't able to register. Make sure your username, email, and password are valid, or try again later.", "Return");
                }
                var signIn = new UserSignIn {
                    UserName = username.Text,
                    Password = password.Text
               };
                await DisplayAlert("Confirm Email", "Please check your email for a confirmation link in order to log in.","Go to Login Page");
               // await _apiAccountService.RequestLogin(signIn);
                await Navigation.PushAsync(new MainPage());
            }
        }
    }
}
