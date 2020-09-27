using MCLiveStatus.Pinger.Models.TcpClients;

namespace MCLiveStatus.Pinger.Models.ServerStatusClients
{
    public class ServerStatusClientFactory
    {
        private readonly CreateTcpClient _createClient;

        public ServerStatusClientFactory(CreateTcpClient createClient)
        {
            _createClient = createClient;
        }

        public ServerStatusClient CreateClient()
        {
            return new ServerStatusClient(_createClient());
        }
    }
}