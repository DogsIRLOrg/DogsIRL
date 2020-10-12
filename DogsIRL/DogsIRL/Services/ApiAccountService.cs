using System;
using DogsIRL.Models;
using Newtonsoft.Json;
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
#if DEBUG
            HttpClientHandler insecureHandler = GetInsecureHandler();
            Client = new HttpClient(insecureHandler);
#else
            Client = new HttpClient();
#endif
        }

        /// <summary>
        /// Requests Login credentials from API and assigns user name and JWT Token
        /// </summary>
        /// <param name="userSignIn"></param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> RequestLogin(UserSignIn userSignIn)
        {
            userSignIn.UserName = userSignIn.UserName.ToUpper();
            var json = JsonConvert.SerializeObject(userSignIn);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync($"{App.ApiUrl}/account/login", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                LoginJwt loginJwt = JsonConvert.DeserializeObject<LoginJwt>(responseContent);
                App.Username = loginJwt.Username;
                App.Token = loginJwt.Jwt;
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebApiRegisterErrors> RequestRegister(RegisterInput input)
        {
            var json = JsonConvert.SerializeObject(input);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync($"{App.ApiUrl}/account/register", httpContent);
            WebApiRegisterErrors errors = new WebApiRegisterErrors();
            if (!response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    errors = JsonConvert.DeserializeObject<WebApiRegisterErrors>(content);
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    WebApiRegisterModelErrors modelErrors = JsonConvert.DeserializeObject<WebApiRegisterModelErrors>(content);
                    List<string> errorList = new List<string>();
                    if(modelErrors.Errors.Username != null)
                    {
                        foreach (string error in modelErrors.Errors.Username)
                        {
                            errorList.Add(error);
                        }
                    }
                    if(modelErrors.Errors.Email != null)
                    {
                        foreach (string error in modelErrors.Errors.Email)
                        {
                            errorList.Add(error);
                        }
                    }
                    if(modelErrors.Errors.Password != null)
                    {
                        foreach (string error in modelErrors.Errors.Password)
                        {
                            errorList.Add(error);
                        }
                    }
                    if(modelErrors.Errors.ConfirmPassword != null)
                    {
                        foreach (string error in modelErrors.Errors.ConfirmPassword)
                        {
                            errorList.Add(error);
                        }
                    }
                    errors.Errors = errorList;
                }
            }
            return errors;
        }

        /// <summary>
        /// Clears cached data for current user
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            App.Username = null;
            App.CurrentDog = null;
            App.Token = null;
        }

        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;

                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            return handler;
        }

        public async Task<HttpResponseMessage> RequestForgotPassword(EmailInput input)
        {
            var json = JsonConvert.SerializeObject(input);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync(
               $"{App.ApiUrl}/account/forgot-password", httpContent);
            return response;
        }

    }
}
