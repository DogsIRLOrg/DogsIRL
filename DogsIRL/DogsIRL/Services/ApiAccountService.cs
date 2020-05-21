using DogsIRL.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DogsIRL.Services
{
    public class ApiAccountService
    {
        HttpClient Client { get; set; }
        
        public ApiAccountService()
        {
            Client = new HttpClient();
        }
        public async Task<string> RequestJwtTokenFromApi()
        {
            HttpResponseMessage result = await Client.GetAsync($"{App.ApiUrl}/main/token");
            if (result.IsSuccessStatusCode)
            {
                string token = await result.Content.ReadAsStringAsync();
                return token;
            }
            return null;
        }

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
        public void Logout()
        {
            App.Token = null;
            App.Username = null;
            App.CurrentDog = null;
        }
        
    }
}
