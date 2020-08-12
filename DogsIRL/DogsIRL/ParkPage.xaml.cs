using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DogsIRL.Models;
using DogsIRL.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DogsIRL
{

    public partial class ParkPage : ContentPage
    {
        public PetCard OtherDog { get; set; }
        private ApiAccountService _apiAccountService { get; set; }

        public ParkPage()
        {
            InitializeComponent();
            _apiAccountService = new ApiAccountService();
        }

        /// <summary>
        /// This method will grab other dog and interact with the current user's dog by calling the GetInteraction method.
        /// </summary>
        protected override async void OnAppearing()
        {
           
            OtherDog = await GetRandomOtherDog(App.Username);
            OtherDogName.Text = OtherDog.Name;
            OtherDogImage.Source = OtherDog.ImageURL;
            CurrentDogName.Text = App.CurrentDog.Name;
            CurrentDogImage.Source = App.CurrentDog.ImageURL;
            GetInteraction(App.CurrentDog, OtherDog);
        }

        /// <summary>
        /// This method will generate random dog. We put owner in param so that it will grab anything other than that user's dog.
        /// </summary>
        public async Task<PetCard> GetRandomOtherDog(string owner)
        {
#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards");
            var pets = JsonConvert.DeserializeObject<List<PetCard>>(response);
            var otherPets = pets.Where(pet => pet.Owner != owner).ToList();

            int randomPetIndex = RandomNumber(0, otherPets.Count);
            
            return otherPets[randomPetIndex];
        }

        /// <summary>
        /// A method to generate random number
        /// </summary>
        int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// This method generate interaction with other dog. We set up 3 lines for hellos conversations and 2 lines for goodbyes. It will be randomly assigned.
        /// </summary>
        /// <param name="currentDog"></param>
        /// <param name="otherDog"></param>
        public async void GetInteraction(PetCard currentDog, PetCard otherDog)
        {
#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/interaction/random");
            Interaction randomInteraction = JsonConvert.DeserializeObject<Interaction>(response);

            LineOne.Text = randomInteraction.OpeningLine;
            LineTwo.Text = randomInteraction.OpeningLineOther;
            LineThree.Text = randomInteraction.ConversationLine;
            LineFour.Text = randomInteraction.GoodbyeLine;
            LineFive.Text = randomInteraction.GoodbyeLineOther;
        }

        /// <summary>
        /// This method will regenerate new interactions with different dog.(may possibly the same dog but different interaction.)
        /// </summary>
        async void OnInteractClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            await AddToCollection(OtherDog.ID);
            var previousPage = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new ParkPage());
            Navigation.RemovePage(previousPage);
            NotBusy();
     
        }

        /// <summary>
        /// This method will add the other dog's petcard that is being interacted with.
        /// </summary>
        public async Task AddToCollection(int petCardId)
        {
#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            CollectInput collectInput = new CollectInput
            {
                Username = App.Username,
                PetCardID = petCardId
            };

            var collectJson = JsonConvert.SerializeObject(collectInput);
            HttpContent collectContent = new StringContent(collectJson);
            collectContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var collectionResponse = await client.PostAsync($"{App.ApiUrl}/collection", collectContent);
        }

        /// <summary>
        /// This method is connected to the button in front end, will add the other dog's petcard that is being interacted with.
        /// </summary>
        async void OnCollectClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            await AddToCollection(OtherDog.ID);
#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            await Navigation.PushAsync(new ProfileView());
            NotBusy();
        }

        /// <summary>
        /// Method to logout.
        /// </summary>
        public async void LogoutClicked(System.Object sender, System.EventArgs e)
        {
           await Navigation.PopToRootAsync();
            _apiAccountService = new ApiAccountService();
           await _apiAccountService.Logout();
        }

        public void Busy()
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;
            btnInteract.IsVisible = false;
            btnCollect.IsVisible = false;
        }

        /// <summary>
        /// Hides spinning loading graphic once upload is complete
        /// </summary>
        public void NotBusy()
        {
            loadingIndicator.IsVisible = false;
            loadingIndicator.IsRunning = false;
            btnInteract.IsVisible = true;
            btnCollect.IsVisible = true;
        }
    }
}
