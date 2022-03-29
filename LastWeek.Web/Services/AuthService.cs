using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LastWeek.Model;
using LastWeek.Web.Model;

namespace LastWeek.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ILocalStorageService localStorage;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
            this.localStorage = localStorage;

            Console.WriteLine("Created AuthService instance.");
            Console.WriteLine($"(AuthService) HttpClient requestheader auth: {httpClient.DefaultRequestHeaders.Authorization}");
        }

        public async Task<User> Register(User registerModel)
        {
            var response = await httpClient.PostAsJsonAsync("Signup", registerModel);
            return response.IsSuccessStatusCode
                ? (await response.Content.ReadFromJsonAsync<User>()) ?? new User { Guid = new Guid() }
                : new User { Guid = new Guid() };
        }

        public async Task<User> Signin(UserSigninInfos signinInfos)
        {
            var response = await httpClient.PostAsJsonAsync("Signin", signinInfos);

            if (!response.IsSuccessStatusCode)
            {
                return new User { Guid = new Guid() };
            }

            var signedInUser = await response.Content.ReadFromJsonAsync<User>();
            if(signedInUser == null)
                return new User { Guid = new Guid() };

            await localStorage.SetItemAsync("authToken", signedInUser.Token);
            await localStorage.SetItemAsync("user", signedInUser);
            Console.WriteLine($"User signed in");
            ((ApiAuthenticationStateProvider)authStateProvider).MarkUserAsAuthenticated(signedInUser.Guid.ToString(), signedInUser.Role);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signedInUser.Token);
            Console.WriteLine($"(AuthService signin) HttpClient requestheader auth: {httpClient.DefaultRequestHeaders.Authorization}");

            return signedInUser;
        }

        public async Task Signout()
        {
            await localStorage.RemoveItemAsync("authToken");
            await localStorage.RemoveItemAsync("user");
            ((ApiAuthenticationStateProvider)authStateProvider).MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
