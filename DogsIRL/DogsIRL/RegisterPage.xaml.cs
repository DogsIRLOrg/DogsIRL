using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DogsIRL.Models;
using DogsIRL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

                if(response == null)
                {
                    await DisplayAlert("Couldn't register", "Oh no we couldn't register! Check the page for any issues with your info!", "Ok");
                    return;
                }
                foreach (IdentityError error in response.Errors)
                {
                    await DisplayAlert("Registration failed", error.Description, "Return");
                }

                await DisplayAlert("Success!", "Please check your email for a confirmation link in order to log in.", "Go to Login Page");
                await Navigation.PopToRootAsync();

            }
        }
    }
}
