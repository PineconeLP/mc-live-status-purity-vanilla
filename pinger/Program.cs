using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Models.TcpClients;
using MCLiveStatus.Pinger.Services;

namespace MCLiveStatus.Pinger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ServerPinger pinger = new ServerPinger(() => new DefaultTcpClient());

            ServerStatus status = await pinger.Ping("purityvanilla.com", 25565);

            Console.WriteLine(status.OnlinePlayers);
        }
    }
}
