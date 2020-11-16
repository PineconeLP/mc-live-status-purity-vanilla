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
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettings;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            IServiceCollection services = builder.Services;

            services.AddScoped<IServerStatusPingerStore, SignalRServerStatusPingerStore>();
            services.AddScoped<IServerPingerSettingsStore, ServerPingerSettingsStore>();

            await builder.Build().RunAsync();
        }
    }
}
