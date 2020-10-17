using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MCLiveStatus.Pinger.Pingers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MCLiveStatus.PurityVanilla.Blazor
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

            services.AddSingleton<RepeatingServerPingerFactory>();
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

            string iconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wwwroot", "icon.ico");
            options.Icon = iconPath;

            if (env.IsProduction())
            {
                options.AutoHideMenuBar = true;
            }

            BrowserWindow window = await Electron.WindowManager.CreateWindowAsync(options);

            window.OnReadyToShow += window.Show;
            window.OnClosed += lifetime.StopApplication;
        }
    }
}
