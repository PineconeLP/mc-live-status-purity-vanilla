using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCLiveStatus.EntityFramework.Models
{
    public class ServerPingerSettingsDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public double PingIntervalSeconds { get; set; }
    }
}