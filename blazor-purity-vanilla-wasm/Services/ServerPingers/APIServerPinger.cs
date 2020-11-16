using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers
{
    public class APIServerPinger : IServerPinger
    {
        private readonly string _pingUrl;

        public APIServerPinger(string pingUrl)
        {
            _pingUrl = pingUrl;
        }

        public async Task<ServerPingResponse> Ping()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_pingUrl);
                string responseJson = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<ServerPingResponse>(responseJson);
            }
        }
    }
}