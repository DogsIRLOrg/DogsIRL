using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DogsIRL.Models;
using DogsIRL.Services;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DogsIRL
{
    public partial class EditPetCard : ContentPage
    {
        private ApiAccountService _apiAccountService { get; set; }

        public EditPetCard()
        {
            InitializeComponent();
            _apiAccountService = new ApiAccountService();
            BringInPetCardCurrentValues();

        }

        private MediaFile _mediaFile;
        private string URL { get; set; }

        private async void btnUpload_Clicked(object sender, EventArgs e)
        {

            if (_mediaFile == null)
            {
                await DisplayAlert("Error", "There was an error when trying to get your image.", "OK");
                return;
            }
            else
            {
                if (await UploadImageAsync(_mediaFile.GetStream(), "temp"))
                {
                    await DisplayAlert("Uploaded", "Image uploaded to Blob Storage Successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Upload failed :(", "Image failed to upload to the server for some reason. Try again later?", "OK");
                }
            }
        }

        private async void btnTakePic_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":(No Camera available.)", "OK");
                return;
            }
            else
            {
                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "myImage.jpg"
                });

                if (_mediaFile == null) return;
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                UploadedUrl.Text = "Image URL:";
            }
        }

        private async void btnSelectPic_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Photos>();

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not supported on your device.", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null) return;
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                UploadedUrl.Text = "Image URL:";
            }
        }

        // Code from https://dzone.com/articles/how-to-upload-images-to-an-aspnet-core-rest-servic
        public async Task<bool> UploadImageAsync(Stream image, string fileName)
        {
            Busy();

#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);

            HttpContent fileStreamContent = new StreamContent(image);
            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileStreamContent);
                var response = await client.PostAsync($"{App.ApiUrl}/image/uploadimage", formData);
                UploadedUrl.Text = await response.Content.ReadAsStringAsync();
                NotBusy();
                return response.IsSuccessStatusCode;
            }
        }

        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            btnSelectPic.IsEnabled = false;
            btnTakePic.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }
        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            btnTakePic.IsEnabled = true;
            btnUpload.IsEnabled = true;
        }

        public async void BringInPetCardCurrentValues()
        {

            int currentDogId = App.CurrentDog.ID;

#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.GetStringAsync($"{App.ApiUrl}/petcards/{currentDogId}");
            var currentPet = JsonConvert.DeserializeObject<PetCard>(response);

            petname.Text = currentPet.Name;

            UploadedUrl.Text = currentPet.ImageURL;
            sex.SelectedIndex = currentPet.Sex == "Male" ? 0 : 1;
            ageyears.Text = currentPet.AgeYears.ToString();
            gooddog.Value = currentPet.GoodDog;
            floofiness.Value = currentPet.Floofiness;
            energy.Value = currentPet.Energy;
            snuggles.Value = currentPet.Snuggles;
            appetite.Value = currentPet.Appetite;
            bravery.Value = currentPet.Bravery;
            imageView.Source = currentPet.ImageURL;
            App.CurrentDog = currentPet;

        }

        public async void LogoutClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();

            _apiAccountService = new ApiAccountService();
            await _apiAccountService.Logout();
        }

        async void OnButtonClicked(System.Object sender, System.EventArgs e)
        {
            var model = new PetCard
            {
                ID = App.CurrentDog.ID,
                Name = petname.Text,
                ImageURL = UploadedUrl.Text,
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
#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.PutAsync(
                $"{App.ApiUrl}/petcards/{model.ID}", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string stringContent = await response.Content.ReadAsStringAsync();
                PetCard postedPetCard = JsonConvert.DeserializeObject<PetCard>(stringContent);
                App.CurrentDog = model;
                await Navigation.PushAsync(new ProfileView());
            }
            else
            {
                await DisplayAlert("Error Updating Pet", "There was an error updating your pet, please try again, but better.", "Return");
            }
        }

        async void OnDeleteButtonClicked(System.Object sender, System.EventArgs e)
        {
            var model = new PetCard
            {
                ID = App.CurrentDog.ID,
                Name = petname.Text,
                ImageURL = UploadedUrl.Text,
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
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{App.ApiUrl}/petcards/{model.ID}");
            request.Content = httpContent;
#if DEBUG
            HttpClientHandler insecureHandler = _apiAccountService.GetInsecureHandler();
            HttpClient client = new HttpClient(insecureHandler);
#else
            HttpClient client = new HttpClient();
#endif
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
            var response = await client.SendAsync(
                request);
            if (response.IsSuccessStatusCode)
            {
                string stringContent = await response.Content.ReadAsStringAsync();
                PetCard postedPetCard = JsonConvert.DeserializeObject<PetCard>(stringContent);
                App.CurrentDog = null;
                await DisplayAlert("Pet Successfully Deleted", $"You have deleted the pet card for {model.Name}", "Return");
                await Navigation.PushAsync(new ProfileView());
            }
            else
            {
                await DisplayAlert("Error Deleting Pet", "There was an error while deleting this pet, please try again.", "Return");
            }
        }
    }
}
