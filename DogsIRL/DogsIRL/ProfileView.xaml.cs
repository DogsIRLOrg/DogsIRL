using System;
using System.Collections.Generic;
using System.Net.Http;
using DogsIRL.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using System.Net.Http.Headers;
using DogsIRL.Services;
using Xamarin.Essentials;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL
{
    public partial class ProfileView : ContentPage
    {
        private List<PetCard> PetList { get; set; }
        private List<PetCard> CollectedList { get; set; }
        private ApiAccountService _apiAccountService { get; set; }


        public ProfileView()
        {
            InitializeComponent();
            Busy();
            _apiAccountService = new ApiAccountService();
            GetPetsOfCurrentUser();
            GetCollectionOfCurrentUser();
            NotBusy();

        }

        /// <summary>
        /// This method will get the current user's pet. If no pet exist, it will redirect to Create Petcard page.
        /// </summary>
        public async void GetPetsOfCurrentUser()
        {

#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards/user/{App.Username}");
            if (response.Length <= 2)
            {
                await Navigation.PushAsync(new CreatePetcard());
                return;
            }

            PetList = JsonConvert.DeserializeObject<List<PetCard>>(response);
            CurrentDog.ItemsSource = PetList;
            if (App.CurrentDog == null)
            {
                App.CurrentDog = PetList[0];
            }
            petCardsList.ItemsSource = PetList;
        }

        /// <summary>
        /// This method will show all the petcards that has been collected by the user after interacting with other pet in the park.
        /// </summary>
        public async void GetCollectionOfCurrentUser()
        {
#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/collection/{App.Username}");
            var collectedPetCardsList = JsonConvert.DeserializeObject<List<CollectedPetCard>>(response);
            CollectedList = new List<PetCard>();
            foreach (CollectedPetCard collectedPetCard in collectedPetCardsList)
            {
                CollectedList.Add(collectedPetCard.PetCard);
            }
            collectionList.ItemsSource = CollectedList;
        }

        /// <summary>
        /// This method is wired with the button to link it to ParkPage.
        /// </summary>
        async void ParkButtonClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            await Navigation.PushAsync(new ParkPage());
            NotBusy();
        }

        /// <summary>
        /// This method is wired with the button to link it to CreatePetCard.
        /// </summary>
        async void PetCardButtonClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            await Navigation.PushAsync(new CreatePetcard());
            NotBusy();
        }

        /// <summary>
        /// This method is wired with the button to link it to Logout.
        /// </summary>
        public async void LogoutClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
            _apiAccountService = new ApiAccountService();
            await _apiAccountService.Logout();
        }

        public async void EditPetCardClicked(System.Object sender, System.EventArgs e)
        {
            Busy();
            Button button = (Button)sender;
            Grid grid = (Grid)button.Parent;
            Label label = (Label)grid.Children[0];
            App.CurrentDog = PetList.Where(card => card.ID.ToString() == label.Text).FirstOrDefault();
            await Navigation.PushAsync(new EditPetCard());
            NotBusy();
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Busy();
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                App.CurrentDog = PetList[selectedIndex];
            }
            NotBusy();
        }

        public async void DonateClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new DonatePage());
        }

        public async void PrivacyButtonClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PrivacyPage());
        }

        public void Busy()
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;
            CurrentDog.IsVisible = false;
            btnGoToPark.IsVisible = false;
            btnAddNewPet.IsVisible = false;
            btnPrivacy.IsVisible = false;
        }

        /// <summary>
        /// Hides spinning loading graphic once upload is complete
        /// </summary>
        public void NotBusy()
        {
            loadingIndicator.IsVisible = false;
            loadingIndicator.IsRunning = false;
            CurrentDog.IsVisible = true;
            btnGoToPark.IsVisible = true;
            btnAddNewPet.IsVisible = true;
            btnPrivacy.IsVisible = true;

        }
    }
}