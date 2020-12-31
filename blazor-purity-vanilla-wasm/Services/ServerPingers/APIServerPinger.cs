using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Endpointer.Core.Models.Responses;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions;
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

        /// <inheritdoc />
        public async Task<ServerPingResponse> Ping()
        {
            HttpResponseMessage response = await _client.GetAsync("");

            if (!response.IsSuccessStatusCode)
            {
                ErrorResponse error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                if (error != null && error.Errors != null && error.Errors.Count() > 0)
                {
                    ErrorMessageResponse firstError = error.Errors.First();
                    if (firstError.Code == 1)
                    {
                        throw new ServerPingFailedException();
                    }
                }

                throw new Exception("Unknown error.");
            }

            return await response.Content.ReadFromJsonAsync<ServerPingResponse>();
        }
    }
}