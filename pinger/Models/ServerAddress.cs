namespace MCLiveStatus.Pinger.Models
{
    public class ServerAddress
    {
        public string Host { get; }
        public int Port { get; }

        public ServerAddress(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}