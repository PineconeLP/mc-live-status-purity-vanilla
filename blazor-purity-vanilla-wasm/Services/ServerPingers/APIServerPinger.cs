using System.Net.Http;
using System.Text.Json;
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
            HttpResponseMessage response = await _client.GetAsync("");
            string responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ServerPingResponse>(responseJson);
        }
    }
}