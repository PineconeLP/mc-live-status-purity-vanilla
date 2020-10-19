using MCLiveStatus.Pinger.Pingers;
using MCLiveStatus.Pinger.Pingers.DinnerbonePython;
using MCLiveStatus.Pinger.Schedulers;
using Microsoft.Extensions.DependencyInjection;

namespace MCLiveStatus.Pinger.Containers
{
    public static class AddServerPingerExtensions
    {
        public static IServiceCollection AddServerPinger(this IServiceCollection services)
        {
            services.AddSingleton<IServerPinger, DinnerbonePythonServerPinger>();
            services.AddSingleton<IServerPingerScheduler, TimerServerPingerScheduler>();
            services.AddSingleton<RepeatingServerPingerFactory>();

            return services;
        }
    }
}