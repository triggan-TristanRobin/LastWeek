using DataManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.MobileBlazorBindings;
using Mobile.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile
{
    public class App : Application
    {
        public App()
        {
            var host = MobileBlazorBindingsHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
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
