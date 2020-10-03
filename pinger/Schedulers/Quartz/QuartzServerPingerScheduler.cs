using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Quartz;

namespace MCLiveStatus.Pinger.Schedulers.Quartz
{
    public class QuartzServerPingerScheduler : IServerPingerScheduler
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServerPinger _serverPinger;

        public QuartzServerPingerScheduler(ISchedulerFactory schedulerFactory, IServerPinger serverPinger)
        {
            _schedulerFactory = schedulerFactory;
            _serverPinger = serverPinger;
        }

        public async Task<StopPingSchedule> Schedule(ServerAddress serverAddress, int secondsInterval = 5, Action<ServerPingResponse> onPing = null)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Start();

            scheduler.JobFactory = new QuartzServerPingerJobFactory(serverAddress, _serverPinger, onPing);

            IJobDetail job = JobBuilder.Create<QuartzServerPingerJob>()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(secondsInterval)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            return () => scheduler.UnscheduleJob(trigger.Key);
        }
    }
}