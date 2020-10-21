namespace MCLiveStatus.PurityVanilla.Blazor.Models
{
    public class PingedServerDetails : IPingedServerDetails
    {
        private readonly ServerDetails _serverDetails;

        public string Host => _serverDetails.Host;
        public int Port => _serverDetails.Port;
        public string Name => _serverDetails.Name;
        public bool HasQueue => _serverDetails.HasQueue;
        public int MaxPlayersExcludingQueue => _serverDetails.MaxPlayersExcludingQueue;
        public int OnlinePlayers { get; set; }
        public int MaxPlayers { get; set; }
        public bool IsFull => OnlinePlayers >= MaxPlayers;
        public bool IsFullExcludingQueue => HasQueue ? OnlinePlayers >= MaxPlayersExcludingQueue : IsFull;

        public PingedServerDetails(ServerDetails serverDetails, int onlinePlayers = 0, int maxPlayers = 0)
        {
            _serverDetails = serverDetails;
        }
    }
}