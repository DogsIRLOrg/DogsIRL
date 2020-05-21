using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DogsIRL.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;

namespace DogsIRL
{
    public partial class CreatePetcard : ContentPage
    {
        public CreatePetcard()
        {

            InitializeComponent();
            CheckAndRequestCameraPermission();
            CheckAndRequestPhotoPermission();
        }

        private MediaFile _mediaFile;
        private string URL { get; set; }

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
        private async void btnUpload_Clicked(object sender, EventArgs e)
        {

            if (_mediaFile == null)
            {
                await DisplayAlert("Error", "There was an error when trying to get your image.", "OK");
                return;
            }
            else
            {
                UploadImage(_mediaFile.GetStream());
            }
        }

        //Take picture from camera    
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

        //Upload to blob function    
        private async void UploadImage(Stream stream)
        {
            Busy();
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=dogsirl;AccountKey=UzCWlAFitDySBsbsndgKVh/hr2IQZz8DJQDPQzG45yhRoAQ2GqMbzifDslOnheiHlSl72smlA3vYncRgbFSIBQ==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("dogs");
            await container.CreateIfNotExistsAsync();
            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{name}.png");
            await blockBlob.UploadFromStreamAsync(stream);
            URL = blockBlob.Uri.OriginalString;
            UploadedUrl.Text = URL;
            NotBusy();
            await DisplayAlert("Uploaded", "Image uploaded to Blob Storage Successfully!", "OK");
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
                    $"{App.ApiUrl}/petcards", httpContent);
                App.CurrentDog = model;
                await Navigation.PushAsync(new ProfileView());
           
        }
    }
}
