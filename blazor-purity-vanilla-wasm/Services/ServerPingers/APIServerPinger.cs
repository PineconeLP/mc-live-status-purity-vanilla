using System.Net.Http;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using RestSharp;

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
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest(_pingUrl, DataFormat.Json);

            return await client.GetAsync<ServerPingResponse>(request);
        }
    }
}