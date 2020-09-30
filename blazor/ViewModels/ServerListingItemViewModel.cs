using System;

namespace MCLiveStatus.Blazor.ViewModels
{
    public class ServerListingItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int OnlinePlayers { get; set; }
        public int MaxPlayersExcludingQueue { get; set; }
        public int MaxPlayers { get; set; }
        public bool IsSelected { get; set; }
        public bool IsFull => OnlinePlayers >= MaxPlayers;
        public bool IsFullExcludingQueue => OnlinePlayers >= MaxPlayersExcludingQueue;
    }
}