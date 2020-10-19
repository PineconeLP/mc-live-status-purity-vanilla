using System;
using System.Threading.Tasks;
using System.Timers;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;

namespace MCLiveStatus.Pinger.Schedulers
{
    /// <summary>
    /// TODO: Adapt Timer class to interface for testing purposes.
    /// </summary>
    public class TimerServerPingerScheduler : IServerPingerScheduler
    {
        private readonly IServerPinger _serverPinger;

        public TimerServerPingerScheduler(IServerPinger serverPinger)
        {
            _serverPinger = serverPinger;
        }

        public Task<IServerPingerSchedulerHandler> Schedule(ServerAddress serverAddress, double secondsInterval,
            Action<ServerPingResponse> onPing = null,
            Action<Exception> onException = null)
        {
            Timer timer = new Timer();

            timer.Interval = secondsInterval * 1000;

            ElapsedEventHandler onTimerElapsed = (s, e) => ExecutePing(serverAddress, onPing, onException);
            timer.Elapsed += onTimerElapsed;

            IServerPingerSchedulerHandler stopPingSchedule = new TimerServerPingerSchedulerHandler(timer);

            timer.Start();

            return Task.FromResult(stopPingSchedule);
        }

        private async void ExecutePing(ServerAddress serverAddress, Action<ServerPingResponse> onPing = null, Action<Exception> onException = null)
        {
            try
            {
                ServerPingResponse response = await _serverPinger.Ping(serverAddress);
                onPing?.Invoke(response);
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }
        }
    }
}