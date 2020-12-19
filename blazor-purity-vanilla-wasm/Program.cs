using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using Append.Blazor.Notifications;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.Notifiers;
using MCLiveStatus.PurityVanilla.Blazor.Services.Notifiers;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using Blazor.Analytics;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers.NotificationPermitters;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettings;
using System.Net.Http;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.NotificationSupportCheckers;
using Endpointer.Authentication.Client.Extensions;
using Endpointer.Authentication.Client.Models;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using Blazored.LocalStorage;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.Tokens;
using Endpointer.Authentication.Client.Stores;
using Refit;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices;
using Endpointer.Authentication.Client.Http;

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

            bool debug = builder.HostEnvironment.IsDevelopment();
            string trackingId = configuration.GetValue<string>("ANALYTICS_GTAG");
            services.AddGoogleAnalytics(trackingId, debug);

            services.AddBlazoredLocalStorage();

            services.AddTransient<ServerDetails>(s => new ServerDetails()
            {
                Host = "purityvanilla.com",
                Port = 25565,
                Name = "Purity Vanilla",
                HasQueue = true,
                MaxPlayersExcludingQueue = 75
            });

            services.AddHttpClient<IServerPinger, APIServerPinger>(client => CreateServerPinger(client, configuration));

            string authenticationBaseUrl = configuration.GetValue<string>("AUTHENTICATION_API_BASE_URL");
            AuthenticationEndpointsConfiguration endpointsConfiguration = new AuthenticationEndpointsConfiguration()
            {
                RegisterEndpoint = authenticationBaseUrl + "register",
                LoginEndpoint = authenticationBaseUrl + "login",
                RefreshEndpoint = authenticationBaseUrl + "refresh",
                LogoutEndpoint = authenticationBaseUrl + "logout"
            };
            services.AddEndpointerAuthenticationClient(endpointsConfiguration, s => s.GetRequiredService<IAutoRefreshTokenStore>());

            services.AddNotifications();
            services.AddScoped<NotificationSupportChecker>();
            services.AddScoped<INotifier, AppendNotifier>();
            services.AddScoped<ServerStatusNotificationFactory>();
            services.AddScoped<ServerStatusNotifier>();

            services.AddScoped<IServerStatusNotificationPermitter, ServerStatusNotificationPermitter>();
            services.Decorate<IServerStatusNotificationPermitter, SettingsStoreServerStatusNotificationPermitter>();

            services.AddScoped<WebStorageTokenStore>();
            services.AddScoped<ITokenStore>(s => s.GetRequiredService<WebStorageTokenStore>());
            services.AddScoped<IAutoRefreshTokenStore>(s => s.GetRequiredService<WebStorageTokenStore>());

            services.AddScoped<AuthenticationStore>();
            services.AddScoped<ServerStatusPingerStoreState>();
            services.AddScoped<IServerStatusPingerStore, SignalRServerStatusPingerStore>(s => CreateServerStatusPingerStore(s, configuration));

            string settingsBaseUrl = configuration.GetValue<string>("SETTINGS_API_BASE_URL");
            services.AddScoped<AutoRefreshHttpMessageHandler>();
            services.AddRefitClient<IAuthenticationServerPingerSettingsService>(new RefitSettings()
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer()
            }).ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(settingsBaseUrl);
            }).AddHttpMessageHandler<AutoRefreshHttpMessageHandler>();
            services.AddScoped<IServerPingerSettingsService, APIServerPingerSettingsService>();

            services.AddScoped<ServerPingerSettingsStore>();
            services.AddScoped<IServerPingerSettingsStore>(s => s.GetRequiredService<ServerPingerSettingsStore>());
            services.AddScoped<IAutoRefreshServerPingerSettingsStore>(s => s.GetRequiredService<ServerPingerSettingsStore>());

            await builder.Build().RunAsync();
        }

        private static SignalRServerStatusPingerStore CreateServerStatusPingerStore(IServiceProvider services, IConfiguration configuration)
        {
            string negotiateUrl = configuration.GetValue<string>("NEGOTIATE_URL");

            return new SignalRServerStatusPingerStore(
                services.GetRequiredService<ServerStatusPingerStoreState>(),
                services.GetRequiredService<ServerPingerSettingsStore>(),
                services.GetRequiredService<IServerPinger>(),
                services.GetRequiredService<ServerStatusNotifier>(),
                negotiateUrl
            );
        }

        private static APIServerPinger CreateServerPinger(HttpClient client, IConfiguration configuration)
        {
            string apiUrl = configuration.GetValue<string>("API_URL");
            client.BaseAddress = new Uri(apiUrl);

            return new APIServerPinger(client);
        }
    }
}
