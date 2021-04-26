using DataManager;
using DataManager.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.MobileBlazorBindings;
using System;
using Xamarin.Forms;

namespace Mobile
{
    public class App : Application
    {
        public App(string[] args = null, IServiceCollection additionalServices = null)
        {
            var host = MobileBlazorBindingsHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    if (additionalServices != null)
                    {
                        services.AddAdditionalServices(additionalServices);
                    }
                    services.AddSingleton<AppState>();
                    services.AddSingleton<IAsyncContentManager>(b => b.GetRequiredService<AppState>().DBManager);
                })
                .Build();

            MainPage = new TabbedPage();
            host.AddComponent<LastWeekApp>(parent: MainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
