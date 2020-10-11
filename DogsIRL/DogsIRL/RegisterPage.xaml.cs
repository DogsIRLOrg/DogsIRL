using System;
using DogsIRL.Models;
using DogsIRL.Services;
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
                Busy();
                var model = new RegisterInput
                {
                    Username = username.Text,
                    Email = email.Text,
                    Password = password.Text,
                    ConfirmPassword = confirmPassword.Text
                };

                var response = await _apiAccountService.RequestRegister(model);
                NotBusy();
                if (response.Errors != null)
                {
                    foreach (string error in response.Errors)
                    {
                        await DisplayAlert("Registration failed", error, "Return");
                    }
                    return;
                }
                await DisplayAlert("Success!", "Please check your email for a confirmation link in order to log in.", "Go to Login Page");
                await Navigation.PopToRootAsync();
            }
        }

        public void Busy()
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;
            btnRegister.IsVisible = false;
        }

        public void NotBusy()
        {
            loadingIndicator.IsVisible = false;
            loadingIndicator.IsRunning = false;
            btnRegister.IsVisible = true;
        }
    }
}
