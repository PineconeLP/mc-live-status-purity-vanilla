using System.Net;
using System.Threading.Tasks;
using Endpointer.Core.Client.Exceptions;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions;
using MCLiveStatus.ServerSettings.Domain.Models;
using Refit;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices
{
    public class ServerPingerSettingsService : IServerPingerSettingsService
    {
        private readonly IAPIServerPingerSettingsService _api;

        public ServerPingerSettingsService(IAPIServerPingerSettingsService api)
        {
            _api = api;
        }

        /// <inheritdoc/>
        public async Task<ServerPingerSettings> GetSettings()
        {
            try
            {
                ServerPingerSettings settings = await _api.GetSettings();

                if (settings == null)
                {
                    throw new ServerPingerSettingsNotFoundException("Server pinger settings were null.");
                }

                return settings;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(ex.Message, ex);
                }

                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new ServerPingerSettingsNotFoundException(ex.Message, ex);
                }

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task SaveSettings(ServerPingerSettings settings)
        {
            try
            {
                await _api.SaveSettings(settings);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException(ex.Message, ex);
                }

                throw;
            }
        }
    }
}