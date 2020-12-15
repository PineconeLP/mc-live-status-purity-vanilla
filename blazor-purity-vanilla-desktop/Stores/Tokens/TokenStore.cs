using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Client.Stores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.Tokens
{
    public class TokenStore : ITokenStore, IAutoRefreshTokenStore
    {
        private string _refreshToken;
        private DateTime _accessTokenExpirationTime;

        public string AccessToken { get; private set; } = string.Empty;

        public bool IsAccessTokenExpired => DateTime.UtcNow > _accessTokenExpirationTime;

        public Task<string> GetRefreshToken()
        {
            return Task.FromResult(_refreshToken);
        }

        public Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime)
        {
            AccessToken = accessToken;
            _refreshToken = refreshToken;
            _accessTokenExpirationTime = accessTokenExpirationTime;

            return Task.CompletedTask;
        }

        public async Task<bool> HasRefreshToken()
        {
            return !string.IsNullOrEmpty(await GetRefreshToken());
        }

        public Task ClearRefreshToken()
        {
            _refreshToken = null;

            return Task.CompletedTask;
        }
    }
}