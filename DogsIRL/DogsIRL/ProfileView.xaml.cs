using System;
using System.Collections.Generic;
using System.Net.Http;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using Xamd.ImageCarousel.Forms.Plugin.Abstractions;

namespace DogsIRL
{
    public partial class ProfileView : ContentPage
    {
        //HttpClient client = new HttpClient();
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();

        public ProfileView()
        {
            InitializeComponent();
            GetPets();
            imageSources.Add("XamarinmonkeyLogo.png");
            imageSources.Add("github.png");
            imageSources.Add("microsoft.png");


            imgSlider.Images = imageSources;
        }

        public async void GetPets()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("https://dogsirl-api.azurewebsites.net/api/petcards");
            var pets = JsonConvert.DeserializeObject<List<PetCard>>(response);
            petCardsList.ItemsSource = pets;
        }
    }
}
