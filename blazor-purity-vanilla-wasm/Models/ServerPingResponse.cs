using System.Text.Json.Serialization;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Models
{
    public class ServerPingResponse
    {
        [JsonPropertyName("online")]
        public int OnlinePlayers { get; set; }

        [JsonPropertyName("max")]
        public int MaxPlayers { get; set; }
    }
}