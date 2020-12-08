using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers
{
    public class APIServerPinger : IServerPinger
    {
        private readonly HttpClient _client;

        public APIServerPinger(HttpClient client)
        {
            _client = client;
        }

        public async Task<ServerPingResponse> Ping()
        {
            return await _client.GetFromJsonAsync<ServerPingResponse>("");
        }
    }
}