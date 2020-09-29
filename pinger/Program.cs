using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.TcpClients;
using MCLiveStatus.Pinger.Pingers;
using Quartz.Impl;
using MCLiveStatus.Pinger.Schedulers;
using MCLiveStatus.Pinger.Schedulers.Quartz;

namespace MCLiveStatus.Pinger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ServerAddress address = new ServerAddress()
            {
                Host = "purityvanilla.com",
                Port = 25565
            };

            CreateTcpClient createClient = () => new DefaultTcpClient();
            ServerNetworkStreamPinger streamPinger = new ServerNetworkStreamPinger();
            ServerPinger pinger = new ServerPinger(createClient, streamPinger);

            ServerPingResponse status = await pinger.Ping(address);
            Console.WriteLine(status.OnlinePlayers);

            IServerPingerScheduler scheduler = new QuartzServerPingerScheduler(new StdSchedulerFactory(), pinger);
            RepeatingServerPinger repeater = new RepeatingServerPinger(address, scheduler);
            repeater.PingCompleted += (s) => Console.WriteLine(s.OnlinePlayers);

            await repeater.Start(3);
            Console.ReadKey();
            await repeater.Stop();
            Console.ReadKey();
        }
    }
}
