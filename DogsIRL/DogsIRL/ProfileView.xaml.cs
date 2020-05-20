using System;
using System.Collections.Generic;
using System.Net.Http;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using Xamd.ImageCarousel.Forms.Plugin.Abstractions;
using System.Net.Http.Headers;

namespace DogsIRL
{
    public partial class ProfileView : ContentPage
    {
        //HttpClient client = new HttpClient();
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        private List<PetCard> PetList { get; set; }

        public ProfileView()
        {
            InitializeComponent();
            GetPets();
        }

        public async void GetPets()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards/user/{App.Username}");
            PetList = JsonConvert.DeserializeObject<List<PetCard>>(response);
            petCardsList.ItemsSource = PetList;

            if(PetList.Count != 0)
            {
                App.CurrentDog = PetList[0];
            }
            else
            {
                await Navigation.PushAsync(new CreatePetcard());
            }
        }

        async void ParkButtonClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ParkPage());
        }
        
            async void PetCardButtonClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreatePetcard());
        }

        public async void LogoutClicked()
        {
            var json = JsonConvert.SerializeObject(App.Username);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(
                "https://dogsirl-api.azurewebsites.net/api/account/logout", httpContent);
            App.Username = null;
            App.CurrentDog = null;
            await Navigation.PushAsync(new MainPage());
        }
    }
}
