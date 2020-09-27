using System.Net.Sockets;
using System.Threading.Tasks;

namespace MCLiveStatus.ServerPinger.Services
{
    public class Pinger
    {
        public async Task Ping(string host, int port)
        {
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(host, port);

                using (NetworkStream stream = client.GetStream())
                {

                }
            }
        }
    }

}