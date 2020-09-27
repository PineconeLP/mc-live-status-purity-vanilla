using System;
using System.Threading.Tasks;
using MCLiveStatus.ServerPinger.Services;

namespace MCLiveStatus.ServerPinger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Pinger pinger = new Pinger();

            await pinger.Ping("purityvanilla.com", 25565);
        }
    }
}
