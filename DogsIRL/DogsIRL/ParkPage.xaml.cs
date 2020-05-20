using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class ParkPage : ContentPage
    {
        public PetCard OtherDog { get; set; }

        public ParkPage()
        {
            InitializeComponent();
            LineOne.Text = "this is the first line";
        }

        protected override async void OnAppearing()
        {
            OtherDog = await GetRandomOtherDog(App.Username);
            OtherDogName.Text = OtherDog.Name;

            CurrentDogName.Text = App.CurrentDog.Name;

        }

        public async Task<PetCard> GetRandomOtherDog(string owner)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards");
            var pets = JsonConvert.DeserializeObject<List<PetCard>>(response);
            var otherPets = pets.Where(pet => pet.Owner != owner).ToList();

            int randomPetIndex = RandomNumber(0, otherPets.Count);
            
            return otherPets[randomPetIndex];
        }

        int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        async void OnInteractClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProfileView());
        }

        async void OnCollectClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProfileView());
        }
    }
}
