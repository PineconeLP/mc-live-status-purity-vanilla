using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Schedulers;

namespace MCLiveStatus.Pinger.Pingers
{
    public class RepeatingServerPinger
    {
        private readonly IServerPingerScheduler _scheduler;
        private readonly ServerAddress _serverAddress;

        private IServerPingerSchedulerHandler _schedulerHandler;

        private bool IsRunning => _schedulerHandler != null && !_schedulerHandler.IsStopped;

        public event Action<ServerPingResponse> PingCompleted;

        public RepeatingServerPinger(ServerAddress serverAddress, IServerPingerScheduler scheduler)
        {
            _serverAddress = serverAddress;
            _scheduler = scheduler;
        }

        public async Task Start(double secondsInterval)
        {
            if (!IsRunning)
            {
                _schedulerHandler = await _scheduler.Schedule(_serverAddress, secondsInterval, OnPingCompleted);
            }
        }

        public async Task UpdateServerPingSecondsInterval(double intervalInSeconds)
        {
            if (IsRunning)
            {
                await _schedulerHandler.UpdatePingScheduleInterval(intervalInSeconds);
            }
        }

        public async Task Stop()
        {
            if (IsRunning)
            {
                await _schedulerHandler.StopPingSchedule();
            }
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            PingCompleted?.Invoke(response);
        }
    }
}