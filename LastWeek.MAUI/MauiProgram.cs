using LastWeek.MAUI.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace LastWeek.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Comfortaa/Comfortaa-VariableFont_wght.ttf", "Comfortaa");
                    fonts.AddFont("Akshar/Akshar-VariableFont_wght.ttf", "Akshar");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<MessageService>();
            builder.Services.AddScoped<ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ApiAuthenticationStateProvider>());
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://lastweek.triggan.com") });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            return builder.Build();
        }
    }
}