namespace MCLiveStatus.PurityVanilla.Blazor.Models
{
    public class ServerStatusPingerSettings
    {
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public double ServerStatusPingIntervalSeconds { get; set; }
    }
}