using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;

namespace MCLiveStatus.Pinger.Pingers
{
    public class RepeatingServerPinger
    {
        public event Action<ServerStatus> PingCompleted;

        public async Task Start()
        {

        }

        public async Task Stop()
        {

        }
    }
}