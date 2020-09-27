using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.TcpClients;
using MCLiveStatus.Pinger.Services;

namespace MCLiveStatus.Pinger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            CreateTcpClient createClient = () => new DefaultTcpClient();
            ServerNetworkStreamPinger streamPinger = new ServerNetworkStreamPinger();
            ServerPinger pinger = new ServerPinger(createClient, streamPinger);

            ServerStatus status = await pinger.Ping("purityvanilla.com", 25565);

            Console.WriteLine(status.OnlinePlayers);
        }
    }
}
