using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Quartz;

namespace MCLiveStatus.Pinger.Schedulers
{
    public class ServerPingerScheduler
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ServerPinger _serverPinger;

        public ServerPingerScheduler(ISchedulerFactory schedulerFactory, ServerPinger serverPinger)
        {
            _schedulerFactory = schedulerFactory;
            _serverPinger = serverPinger;
        }

        public async Task<Func<Task>> Start(ServerAddress serverAddress, Action<ServerPingResponse> onPing)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Start();

            scheduler.JobFactory = new ServerPingerJobFactory(serverAddress, _serverPinger, onPing);

            IJobDetail job = JobBuilder.Create<ServerPingerJob>()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(3)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            return () => scheduler.UnscheduleJob(trigger.Key);
        }
    }
}