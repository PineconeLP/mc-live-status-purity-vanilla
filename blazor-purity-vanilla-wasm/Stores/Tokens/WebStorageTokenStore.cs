using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Endpointer.Core.Client.Stores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.Tokens
{
    public class WebStorageTokenStore : AutoRefreshTokenStoreBase, ITokenStore
    {
        private const string REFRESH_TOKEN_KEY = "refresh";

        private readonly ILocalStorageService _localStorage;

        private string _accessToken;

        public override string AccessToken => _accessToken;

        public WebStorageTokenStore(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<string> GetRefreshToken()
        {
            return await _localStorage.GetItemAsStringAsync(REFRESH_TOKEN_KEY);
        }

        public override async Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime)
        {
            _accessToken = accessToken;
            AccessTokenExpirationTime = accessTokenExpirationTime;

            await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, refreshToken);
        }

        public async Task<bool> HasRefreshToken()
        {
            return !string.IsNullOrEmpty(await GetRefreshToken());
        }

        public async Task ClearRefreshToken()
        {
            await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
        }
    }
}