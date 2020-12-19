using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;
using MCLiveStatus.ServerSettings.Domain.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices
{
    public class APIServerPingerSettingsService : IServerPingerSettingsService
    {
        private readonly IAuthenticationServerPingerSettingsService _settingsService;
        private readonly ITokenStore _tokenStore;

        private string AccessToken => $"Bearer {_tokenStore.AccessToken}";

        public APIServerPingerSettingsService(IAuthenticationServerPingerSettingsService settingsService, ITokenStore tokenStore)
        {
            _settingsService = settingsService;
            _tokenStore = tokenStore;
        }

        public async Task<ServerPingerSettings> GetSettings()
        {
            return await _settingsService.GetSettings(AccessToken);
        }

        public async Task SaveSettings(ServerPingerSettings settings)
        {
            await _settingsService.SaveSettings(AccessToken, settings);
        }
    }
}