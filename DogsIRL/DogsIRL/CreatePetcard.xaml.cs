using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class CreatePetcard : ContentPage
    {
        public CreatePetcard()
        {

            InitializeComponent();
            
        }

        async void OnButtonClicked(System.Object sender, System.EventArgs e)
        {

            var model = new PetCard
            {
                Name = petname.Text,
                Owner = App.Username,
                Sex = sex.SelectedIndex == 0 ? "Male" : "Female",
                AgeYears = int.Parse(ageyears.Text),
                GoodDog = (sbyte)gooddog.Value,
                Floofiness = (sbyte)floofiness.Value,
                Energy = (sbyte)energy.Value,
                Snuggles = (sbyte)snuggles.Value,
                Appetite = (sbyte)appetite.Value,
                Bravery = (sbyte)bravery.Value
                    
                };
                var json = JsonConvert.SerializeObject(model);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var client = new HttpClient();
                var response = await client.PostAsync(
                    "https://dogsirl-api.azurewebsites.net/api/petcards", httpContent);
                App.CurrentDog = model;
                await Navigation.PushAsync(new ProfileView());
           
        }
    }
}
