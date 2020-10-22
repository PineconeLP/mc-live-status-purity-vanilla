using System;
using System.ComponentModel.DataAnnotations;

namespace MCLiveStatus.EntityFramework.Models
{
    public class ServerPingerSettingsDTO
    {
        [Key]
        public Guid Id { get; set; }
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public double PingIntervalSeconds { get; set; }
    }
}