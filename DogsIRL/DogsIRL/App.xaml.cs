﻿using System;
using System.IO;
using DogsIRL.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DogsIRL
{
    public partial class App : Application
    {
        public static string ApiUrl = "https://dogsirl-api.azurewebsites.net/api";
        public static string Username;
        public static PetCard CurrentDog;

        public App()
        {
            Device.SetFlags(new string[] { "RadioButton_Experimental" });
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
