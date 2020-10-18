using Newtonsoft.Json;

namespace MCLiveStatus.Pinger.Pingers.DinnerbonePython
{
    public class ServerPlayers
    {
        [JsonProperty("online")]
        public int Online { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }
    }
}