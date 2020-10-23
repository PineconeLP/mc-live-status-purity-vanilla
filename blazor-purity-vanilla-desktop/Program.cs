using ElectronNET.API;
using MCLiveStatus.EntityFramework.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            MCLiveStatusDbContextFactory contextFactory = host.Services.GetRequiredService<MCLiveStatusDbContextFactory>();
            using (MCLiveStatusDbContext context = contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseElectron(args);
                    webBuilder.UseStartup<Startup>();
                });
    }
}
