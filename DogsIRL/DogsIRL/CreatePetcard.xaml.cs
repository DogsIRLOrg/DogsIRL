using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DogsIRL.Models;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Essentials;
using DogsIRL.Services;

namespace DogsIRL
{
    public partial class CreatePetcard : ContentPage
    {
        private ApiAccountService _apiAccountService { get; set; }

        public CreatePetcard()
        {

            InitializeComponent();
            _apiAccountService = new ApiAccountService();
            CheckAndRequestCameraPermission();
            CheckAndRequestPhotoPermission();
        }

        private MediaFile _mediaFile;
        private string URL { get; set; }

        /// <summary>
        /// When Page is loaded, checks if app has permission for camera use. If not, requests permission.
        /// </summary>
        /// <returns>Permission status</returns>
        public async Task<PermissionStatus> CheckAndRequestCameraPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            // Additionally could prompt the user to turn on in settings

            return status;
        }

        /// <summary>
        /// When Page is loaded, checks if app has permission for photo gallery use. If not, requests permission.
        /// </summary>
        /// <returns>Permission status</returns>
        public async Task<PermissionStatus> CheckAndRequestPhotoPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Photos>();
            }

            // Additionally could prompt the user to turn on in settings

            return status;
        }
        /// <summary>
        /// Checks permissions, if granted will open photo gallery and allow user to select image to be uploaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //Upload picture button
        /// <summary>
        /// Prepares image url to be uploaded to blob storage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpload_Clicked(object sender, EventArgs e)
        {

            if (_mediaFile == null)
            {
                await DisplayAlert("Error", "There was an error when trying to get your image.", "OK");
                return;
            }
            else
            {

                if (await UploadImageAsync(_mediaFile.GetStream(), $"{App.Username}(TempImage)")) {
                    await DisplayAlert("Uploaded", "Image uploaded to Blob Storage Successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Upload failed :(", "Image failed to upload to the server for some reason. Try again later?", "OK");
                }
            }
        }

        //Take picture from camera
        /// <summary>
        /// Checks camera permission, if granted will open users camera and allow image to be captured and prepared for blob storage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                var response = await client.PostAsync($"{App.ApiUrl}/petcards/uploadimage", formData);
                UploadedUrl.Text = await response.Content.ReadAsStringAsync();
                NotBusy();
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Displays spinning loading graphic while uploading image
        /// </summary>
        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            btnSelectPic.IsEnabled = false;
            btnTakePic.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }

        /// <summary>
        /// Hides spinning loading graphic once upload is complete
        /// </summary>
        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            btnTakePic.IsEnabled = true;
            btnUpload.IsEnabled = true;
        }
    
        /// <summary>
        /// Takes in the inout data from user and creates a new PetCard with assigned values. Saves new PetCard to database. Adds new PetCard to current users collected pets.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnButtonClicked(System.Object sender, System.EventArgs e)
        {

            var model = new PetCard
            {
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
                var response = await client.PostAsync(
                    $"{App.ApiUrl}/petcards", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string stringContent = await response.Content.ReadAsStringAsync();
                PetCard postedPetCard = JsonConvert.DeserializeObject<PetCard>(stringContent);
                await AddToCollection(postedPetCard.ID);
                App.CurrentDog = model;
                await Navigation.PushAsync(new ProfileView());
            }
            else
            {
                await DisplayAlert("Error Adding Pet", "There was an error adding your pet, please try again, but better.", "Return");
            }
        }
        /// <summary>
        /// Logs current user out and clears the Navigation stack back to the root level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void LogoutClicked(System.Object sender, System.EventArgs e)
        {
            //var existingPages = Navigation.NavigationStack.ToList();
            //foreach (var page in existingPages)
            //{
            //    var previousPage = Navigation.NavigationStack.LastOrDefault();
            //    Navigation.RemovePage(previousPage);
            //}
            await Navigation.PopToRootAsync();

            _apiAccountService = new ApiAccountService();
            await _apiAccountService.Logout();
        }

        /// <summary>
        /// Adds the current dog being created to the current users collected dogs.
        /// </summary>
        /// <param name="petCardId"></param>
        /// <returns></returns>
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
    }

}
