using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Endpointer.Authentication.Client.Stores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.Tokens
{
    public class WebStorageTokenStore : ITokenStore, IAutoRefreshTokenStore
    {
        private const string REFRESH_TOKEN_KEY = "refresh";

        private readonly ILocalStorageService _localStorage;

        private DateTime _accessTokenExpirationTime;

        public string AccessToken { get; private set; }

        public bool IsAccessTokenExpired => DateTime.UtcNow > _accessTokenExpirationTime;

        public WebStorageTokenStore(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string> GetRefreshToken()
        {
            return await _localStorage.GetItemAsStringAsync(REFRESH_TOKEN_KEY);
        }

        public async Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime)
        {
            AccessToken = accessToken;
            await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, refreshToken);
            _accessTokenExpirationTime = accessTokenExpirationTime;
        }

        public async Task<bool> HasRefreshToken()
        {
            return !string.IsNullOrEmpty(await GetRefreshToken());
        }
    }
}