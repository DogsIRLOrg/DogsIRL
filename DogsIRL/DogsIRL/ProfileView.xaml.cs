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

namespace DogsIRL
{
    public partial class ProfileView : ContentPage
    {
        //HttpClient client = new HttpClient();
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        private List<PetCard> PetList { get; set; }
        private ApiAccountService _apiAccountService { get; set; }

        public ProfileView()
        {
            InitializeComponent();
            GetPetsOfCurrentUser();
            
        }

        public async void GetPetsOfCurrentUser()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards/user/{App.Username}");
            if (response.Length == 0)
            {
                await Navigation.PushAsync(new CreatePetcard());
            }
            
            PetList = JsonConvert.DeserializeObject<List<PetCard>>(response);
            if (App.CurrentDog == null)
            {
                App.CurrentDog = PetList[0];
            }
            petCardsList.ItemsSource = PetList;
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
            var existingPages = Navigation.NavigationStack.ToList();
            foreach (var page in existingPages)
            {
            var previousPage = Navigation.NavigationStack.LastOrDefault();
                Navigation.RemovePage(previousPage);
            }
            _apiAccountService = new ApiAccountService();
            _apiAccountService.Logout();
            //await Navigation.PushAsync(new MainPage());
        }
    }
}
