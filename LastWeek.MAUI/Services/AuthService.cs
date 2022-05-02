using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Text.Json;
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
        public User User { get; private set; }

        public AuthService(HttpClient httpClient, ApiAuthenticationStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
            //SetSignedInUser().Wait();

            Console.WriteLine("Created AuthService instance.");
            Console.WriteLine($"(AuthService) HttpClient requestheader auth: {httpClient.DefaultRequestHeaders.Authorization}");
        }

        public async Task SetSignedInUser()
        {
            try
            {
                var jsonUser = await SecureStorage.GetAsync("user");
                if(jsonUser != null)
                {
                    User = JsonSerializer.Deserialize<User>(jsonUser);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            await SecureStorage.SetAsync("user", JsonSerializer.Serialize(signedInUser));
            Console.WriteLine($"User signed in");
            authStateProvider.MarkUserAsAuthenticated(signedInUser.Guid.ToString(), signedInUser.Role);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", signedInUser.Token);
            Console.WriteLine($"(AuthService signin) HttpClient requestheader auth: {httpClient.DefaultRequestHeaders.Authorization}");
            User = signedInUser;

            return signedInUser;
        }

        public async Task Signout()
        {
            SecureStorage.Remove("authToken");
            SecureStorage.Remove("user");
            authStateProvider.MarkUserAsLoggedOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
