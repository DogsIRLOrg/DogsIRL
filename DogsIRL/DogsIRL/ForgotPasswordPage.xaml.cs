using System;
using System.Collections.Generic;
using DogsIRL.Models;
using DogsIRL.Services;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly ApiAccountService _apiAccountService;

        public ForgotPasswordPage()
        {
            InitializeComponent();
            _apiAccountService = new ApiAccountService();
        }

        async void ForgotPasswordButtonOnClicked(System.Object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                var model = new EmailInput
                {
                    Email = EmailEntry.Text
                };

                var response = await _apiAccountService.RequestForgotPassword(model);

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Forgot Password Server Error", "Please try again later.", "Return");
                }

                await DisplayAlert("Request sent", "Please check your email for a link to reset your password. Your email must be confirmed to reset your password.", "Return to Login Page");
                await Navigation.PopToRootAsync();

            }
        }
    }
}
