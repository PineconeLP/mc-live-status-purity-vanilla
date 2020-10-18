using System;
using System.Threading.Tasks;
using System.Timers;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;

namespace MCLiveStatus.Pinger.Schedulers
{
    public class TimerServerPingerScheduler : IServerPingerScheduler
    {
        private readonly IServerPinger _serverPinger;

        public TimerServerPingerScheduler(IServerPinger serverPinger)
        {
            _serverPinger = serverPinger;
        }

        public Task<StopPingSchedule> Schedule(ServerAddress serverAddress, double secondsInterval,
            Action<ServerPingResponse> onPing = null,
            Action<Exception> onException = null)
        {
            Timer timer = new Timer();

            timer.Interval = secondsInterval * 1000;

            ElapsedEventHandler onTimerElapsed = (s, e) => ExecutePing(serverAddress, onPing, onException);
            timer.Elapsed += onTimerElapsed;

            StopPingSchedule stopPingSchedule = () => Task.Run(timer.Dispose);

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