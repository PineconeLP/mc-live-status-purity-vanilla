namespace MCLiveStatus.PurityVanilla.Blazor.Models
{
    public class ServerDetails
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public bool HasQueue { get; set; }
        public int MaxPlayersExcludingQueue { get; set; }
    }
}