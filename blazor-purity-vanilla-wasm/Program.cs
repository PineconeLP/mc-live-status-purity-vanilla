using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using Append.Blazor.Notifications;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.Notifiers;
using MCLiveStatus.PurityVanilla.Blazor.Services.Notifiers;
using MCLiveStatus.PurityVanilla.Blazor.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            IConfiguration configuration = builder.Configuration;
            IServiceCollection services = builder.Services;

            services.AddNotifications();
            services.AddScoped<INotifier, AppendNotifier>();
            services.AddScoped<ServerStatusNotificationFactory>();
            services.AddScoped<ServerStatusNotifier>();

            services.AddScoped<IServerStatusPingerStore, SignalRServerStatusPingerStore>(s => CreateServerStatusPingerStore(s, configuration));
            services.AddSingleton<IServerPinger>(CreateServerPinger(configuration));

            await builder.Build().RunAsync();
        }

        private static SignalRServerStatusPingerStore CreateServerStatusPingerStore(IServiceProvider services, IConfiguration configuration)
        {
            string negotiateUrl = configuration.GetValue<string>("NEGOTIATE_URL");

            return new SignalRServerStatusPingerStore(
                services.GetRequiredService<IServerPinger>(),
                services.GetRequiredService<ServerStatusNotifier>(),
                negotiateUrl
            );
        }

        private static APIServerPinger CreateServerPinger(IConfiguration configuration)
        {
            string apiUrl = configuration.GetValue<string>("API_URL");

            return new APIServerPinger(apiUrl);
        }
    }
}
