using DogsIRL.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DogsIRL.Services
{
    public class ApiAccountService
    {
        HttpClient Client { get; set; }
      
        public ApiAccountService()
        {
            Client = new HttpClient();
        }
        /// <summary>
        /// Recieves a unique jwt token from the backend API to be used for authorization
        /// </summary>
        /// <returns>JWT Token as a String</returns>
        public async Task<string> RequestJwtTokenFromApi()
        {
            HttpResponseMessage result = await Client.GetAsync($"{App.ApiUrl}/main/token");
            if (result.IsSuccessStatusCode)
            {
                string token = await result.Content.ReadAsStringAsync();
                return token;
            }
           // string token = result.ToString();
            return null;
        }
        /// <summary>
        /// Requests Login credentials from API and assigns user name and JWT Token
        /// </summary>
        /// <param name="userSignIn"></param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> RequestLogin(UserSignIn userSignIn)
        {
            var json = JsonConvert.SerializeObject(userSignIn);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync($"{App.ApiUrl}/account/login", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                App.Username = userSignIn.UserName;
                string token = await RequestJwtTokenFromApi();
                 App.Token = token;
            }
            return response;
        }
        /// <summary>
        /// Clears cached data for current user
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            App.Username = null;
            App.CurrentDog = null;
            App.Token = null;
        }
        
    }
}
