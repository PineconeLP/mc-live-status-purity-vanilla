namespace MCLiveStatus.Domain.Models
{
    public class ServerPingerSettings
    {
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public double PingIntervalSeconds { get; set; }
    }
}