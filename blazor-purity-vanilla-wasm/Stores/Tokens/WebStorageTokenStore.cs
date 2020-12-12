using System.Threading.Tasks;
using Blazored.LocalStorage;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.Tokens
{
    public class WebStorageTokenStore : ITokenStore
    {
        private const string REFRESH_TOKEN_KEY = "refresh";

        private readonly ILocalStorageService _localStorage;

        public string AccessToken { get; set; }

        public WebStorageTokenStore(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }


        public void ClearAccessToken()
        {
            AccessToken = null;
        }

        public async Task ClearRefreshToken()
        {
            await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
        }

        public async Task<string> GetRefreshToken()
        {
            return await _localStorage.GetItemAsStringAsync(REFRESH_TOKEN_KEY);
        }

        public async Task SetRefreshToken(string refreshToken)
        {
            await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, refreshToken);
        }
    }
}