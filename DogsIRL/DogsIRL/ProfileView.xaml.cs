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
using DogsIRL.Services;
using Xamarin.Essentials;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL
{
    public partial class ProfileView : ContentPage
    {
        //HttpClient client = new HttpClient();
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        private List<PetCard> PetList { get; set; }
        private List<PetCard> CollectedList { get; set; }
        private ApiAccountService _apiAccountService { get; set; }


        public ProfileView()
        {
            InitializeComponent();
            GetPetsOfCurrentUser();
            GetCollectionOfCurrentUser();
            
        }

        public async void GetPetsOfCurrentUser()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards/user/{App.Username}");
            if (response.Length <= 2)
            {
                await Navigation.PushAsync(new CreatePetcard());
                return;
            }
            
            PetList = JsonConvert.DeserializeObject<List<PetCard>>(response);
            if (App.CurrentDog == null)
            {
                App.CurrentDog = PetList[0];
            }
            petCardsList.ItemsSource = PetList;
        }

        public async void GetCollectionOfCurrentUser()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/collection/{App.Username}");
            var collectedPetCardsList = JsonConvert.DeserializeObject<List<CollectedPetCard>>(response);
            CollectedList = new List<PetCard>();
            foreach(CollectedPetCard collectedPetCard in collectedPetCardsList)
            {
                CollectedList.Add(collectedPetCard.PetCard);
            }
            collectionList.ItemsSource = CollectedList;
        }

        async void ParkButtonClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ParkPage());
        }
        
            async void PetCardButtonClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CreatePetcard());
        }

        public async void LogoutClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
            _apiAccountService = new ApiAccountService();
           await _apiAccountService.Logout();
        }
    }
}
