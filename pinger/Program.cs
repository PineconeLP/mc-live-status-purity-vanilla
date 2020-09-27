using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Models.ServerStatusClients;
using MCLiveStatus.Pinger.Models.TcpClients;
using MCLiveStatus.Pinger.Services;

namespace MCLiveStatus.Pinger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ServerStatusClientFactory clientFactory = new ServerStatusClientFactory(() => new DefaultTcpClient());
            ServerPinger pinger = new ServerPinger(clientFactory);

            ServerStatus status = await pinger.Ping("purityvanilla.com", 25565);

            Console.WriteLine(status.OnlinePlayers);
        }
    }
}
