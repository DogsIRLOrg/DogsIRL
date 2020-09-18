using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using DogsIRL.Services;

namespace DogsIRL
{
    
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ApiAccountService ApiAccountService { get; set; }
        public MainPage()
        {
            InitializeComponent();
            ApiAccountService = new ApiAccountService();
        }

        /// <summary>
        /// Checks input username and password, if a match is found in database, logs user in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnButtonClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            var signInUser = new UserSignIn()
            {
                UserName = NameEntry.Text,
                Password = Password.Text,
            };
            var result = await ApiAccountService.RequestLogin(signInUser);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //var token = await ApiAccountService.RequestJwtTokenFromApi();
                //App.Token = token;
                await Navigation.PushAsync(new ProfileView());
            }
            else
            {
                await DisplayAlert("Failed Login Attempt", "Something has gone horribly wrong. Its probably your fault", "Try Again");
            }
            NotBusy();
        }

        /// <summary>
        /// Takes current user to a register page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void RegisterButtonOnClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            await Navigation.PushAsync(new RegisterPage());
            NotBusy();
        }

        async void ForgotPasswordButtonOnClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            await Navigation.PushAsync(new ForgotPasswordPage());
            NotBusy();
        }

        async void PrivacyButtonOnClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PrivacyPage());
        }

        public void Busy()
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;
            textOr.IsVisible = false;
            btnCreate.IsVisible = false;
            btnSignIn.IsVisible = false;
            btnForgot.IsVisible = false;
            btnPrivacy.IsVisible = false;
            btnCreate.IsEnabled = false;
            btnForgot.IsEnabled = false;
            btnSignIn.IsEnabled = false;
            btnPrivacy.IsEnabled = false;
        }

        /// <summary>
        /// Hides spinning loading graphic once upload is complete
        /// </summary>
        public void NotBusy()
        {
            loadingIndicator.IsVisible = false;
            loadingIndicator.IsRunning = false;
            textOr.IsVisible = true;
            btnCreate.IsVisible = true;
            btnSignIn.IsVisible = true;
            btnForgot.IsVisible = true;
            btnPrivacy.IsVisible = true;
            btnCreate.IsEnabled = true;
            btnForgot.IsEnabled = true;
            btnSignIn.IsEnabled = true;
            btnPrivacy.IsEnabled = true;
        }
    }
}
