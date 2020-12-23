using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MCLiveStatus.Pinger.Containers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MCLiveStatus.EntityFramework.Containers;
using Microsoft.EntityFrameworkCore;
using MCLiveStatus.EntityFramework.Mappers;
using AutoMapper;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Services.Notifiers;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.Notifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers.NotificationPermitters;
using Endpointer.Authentication.Client.Models;
using Endpointer.Authentication.Client.Extensions;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.Tokens;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using Endpointer.Core.Client.Stores;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddAutoMapper(typeof(DTOMappingProfile));
            services.AddDataServices(o => o.UseSqlite("Data Source=MCLiveStatus.db"));

            services.AddSingleton<TokenStore>();
            services.AddSingleton<ITokenStore>(s => s.GetRequiredService<TokenStore>());
            services.AddSingleton<IAutoRefreshTokenStore>(s => s.GetRequiredService<TokenStore>());

            string authenticationBaseUrl = Configuration.GetValue<string>("AUTHENTICATION_API_BASE_URL");
            AuthenticationEndpointsConfiguration endpointsConfiguration = new AuthenticationEndpointsConfiguration()
            {
                RegisterEndpoint = authenticationBaseUrl + "register",
                LoginEndpoint = authenticationBaseUrl + "login",
                RefreshEndpoint = authenticationBaseUrl + "refresh",
                LogoutEndpoint = authenticationBaseUrl + "logout"
            };
            services.AddEndpointerAuthenticationClient(endpointsConfiguration, s => s.GetRequiredService<IAutoRefreshTokenStore>());

            services.AddTransient<ServerDetails>(s => new ServerDetails()
            {
                Host = "purityvanilla.com",
                Port = 25565,
                Name = "Purity Vanilla",
                HasQueue = true,
                MaxPlayersExcludingQueue = 75
            });

            services.AddSingleton<IServerStatusNotificationPermitter, ServerStatusNotificationPermitter>();
            services.Decorate<IServerStatusNotificationPermitter, SettingsStoreServerStatusNotificationPermitter>();

            services.AddServerPinger();
            services.AddSingleton<ServerStatusNotificationFactory>();
            services.AddSingleton<INotifier, ElectronNotifier>();
            services.AddSingleton<IServerStatusNotifier, ServerStatusNotifier>();
            services.AddSingleton<ServerStatusPingerStoreState>();
            services.AddSingleton<IServerStatusPingerStore, ServerStatusPingerStore>();

            services.AddSingleton<AuthenticationStore>();
            services.AddSingleton<ServerPingerSettingsStore>();
            services.AddSingleton<IPingConfigurableServerPingerSettingsStore>(s => s.GetRequiredService<ServerPingerSettingsStore>());
            services.AddSingleton<IServerPingerSettingsStore>(s => s.GetRequiredService<ServerPingerSettingsStore>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            StartElectron(env, lifetime);
        }

        private async void StartElectron(IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            BrowserWindowOptions options = new BrowserWindowOptions();

            // Default to not showing in order to manually control show/hide status.
            //
            // This needs to be manually controlled in order to properly stop the application
            // when the window is closed.
            options.Show = false;

            if (env.IsProduction())
            {
                options.AutoHideMenuBar = true;
                options.Icon = GetIconPath();
            }

            BrowserWindow window = await Electron.WindowManager.CreateWindowAsync(options);

            window.OnReadyToShow += window.Show;
            window.OnClosed += lifetime.StopApplication;
        }

        private string GetIconPath()
        {
            string iconFileName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                iconFileName = "icon.ico";
            }
            else
            {
                iconFileName = "icon.jpg";
            }

            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wwwroot", iconFileName);
        }
    }
}
