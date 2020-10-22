using System;

namespace MCLiveStatus.Domain.Models
{
    public class ServerPingerSettings
    {
        public Guid Id { get; }
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public double PingIntervalSeconds { get; set; }

        public ServerPingerSettings() : this(Guid.Empty) { }

        public ServerPingerSettings(Guid id)
        {
            Id = id;
        }
    }
}