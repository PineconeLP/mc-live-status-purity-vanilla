using System;

namespace MCLiveStatus.ServerSettings.Domain.Models
{
    public class ServerPingerSettings
    {
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public bool AutoRefresh { get; set; }
    }
}