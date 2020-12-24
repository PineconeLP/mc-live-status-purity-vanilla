using System;
using MCLiveStatus.Domain.Services;
using MCLiveStatus.EntityFramework.Contexts;
using MCLiveStatus.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MCLiveStatus.EntityFramework.Containers
{
    public static class AddDataServicesExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDbContext)
        {
            services.AddDbContext<MCLiveStatusDbContext>(configureDbContext);
            services.AddSingleton<MCLiveStatusDbContextFactory>(new MCLiveStatusDbContextFactory(configureDbContext));

            services.AddSingleton<IRefreshTokenRepository, EFRefreshTokenRepository>();
            services.AddSingleton<IServerPingerSettingsRepository, EFServerPingerSettingsRepository>();

            return services;
        }
    }
}