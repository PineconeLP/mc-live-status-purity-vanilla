using System;

namespace MCLiveStatus.Domain.Models
{
    public class ServerPingerSettings
    {
        public int Id { get; }
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public double PingIntervalSeconds { get; set; }

        public ServerPingerSettings() : this(0) { }

        public ServerPingerSettings(int id)
        {
            Id = id;
        }
    }
}