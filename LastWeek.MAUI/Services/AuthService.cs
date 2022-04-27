using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LastWeek.Model;
using LastWeek.MAUI.Model;

namespace LastWeek.MAUI.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ApiAuthenticationStateProvider authStateProvider;

        public AuthService(HttpClient httpClient, ApiAuthenticationStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;

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

            await SecureStorage.SetAsync("authToken", signedInUser.Token);
            Console.WriteLine($"User signed in");
            authStateProvider.MarkUserAsAuthenticated(signedInUser.Guid.ToString(), signedInUser.Role);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signedInUser.Token);
            Console.WriteLine($"(AuthService signin) HttpClient requestheader auth: {httpClient.DefaultRequestHeaders.Authorization}");

            return signedInUser;
        }

        public async Task Signout()
        {
            SecureStorage.Remove("authToken");
            authStateProvider.MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
