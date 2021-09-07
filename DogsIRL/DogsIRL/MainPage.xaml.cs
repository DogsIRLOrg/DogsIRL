using System;
using System.ComponentModel;
using DogsIRL.Models;
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
        readonly INotificationRegistrationService _notificationRegistrationService;
        public MainPage()
        {
            InitializeComponent();
            ApiAccountService = new ApiAccountService();
            _notificationRegistrationService =
        ServiceContainer.Resolve<INotificationRegistrationService>();
        }

        protected override async void OnAppearing()
        {
            Password.Text = String.Empty;
            ApiAccountService.Logout();
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
                await _notificationRegistrationService.RegisterDeviceAsync().ContinueWith((task)
        => {
            DisplayAlert("Notification Status", task.IsFaulted ?
               task.Exception.Message :
               $"Device registered", "Ok");
        });
                await Navigation.PushAsync(new ProfileView());
            }
            else
            {
                await DisplayAlert("Failed Login Attempt", "Oh no! Check your username and password are correct, and that you've confirmed your email.", "Try Again");
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
            btnCreate.IsVisible = false;
            btnSignIn.IsVisible = false;
            btnForgot.IsVisible = false;
            btnPrivacy.IsVisible = false;
            btnCreate.IsEnabled = false;
            btnForgot.IsEnabled = false;
            btnSignIn.IsEnabled = false;
            btnPrivacy.IsEnabled = false;
            labelVersion.IsVisible = false;
        }

        /// <summary>
        /// Hides spinning loading graphic once upload is complete
        /// </summary>
        public void NotBusy()
        {
            loadingIndicator.IsVisible = false;
            loadingIndicator.IsRunning = false;
            btnCreate.IsVisible = true;
            btnSignIn.IsVisible = true;
            btnForgot.IsVisible = true;
            btnPrivacy.IsVisible = true;
            btnCreate.IsEnabled = true;
            btnForgot.IsEnabled = true;
            btnSignIn.IsEnabled = true;
            btnPrivacy.IsEnabled = true;
            labelVersion.IsVisible = true;
        }
    }
}
