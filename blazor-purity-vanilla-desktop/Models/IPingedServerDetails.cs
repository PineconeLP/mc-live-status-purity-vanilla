namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Models
{
    public interface IPingedServerDetails
    {
        string Host { get; }
        int Port { get; }
        string Name { get; }
        bool HasQueue { get; }
        int MaxPlayersExcludingQueue { get; }
        int OnlinePlayers { get; }
        int MaxPlayers { get; }
        bool IsFull { get; }
        bool IsFullExcludingQueue { get; }
        bool HasData { get; }
    }
}