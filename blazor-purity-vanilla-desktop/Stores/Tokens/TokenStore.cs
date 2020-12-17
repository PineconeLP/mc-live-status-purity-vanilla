using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Client.Stores;
using MCLiveStatus.Domain.Services;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.Tokens
{
    public class TokenStore : ITokenStore, IAutoRefreshTokenStore
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private DateTime _accessTokenExpirationTime;

        public string AccessToken { get; private set; } = string.Empty;

        public bool IsAccessTokenExpired => DateTime.UtcNow > _accessTokenExpirationTime;

        public TokenStore(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> GetRefreshToken()
        {
            return await _refreshTokenRepository.GetRefreshToken();
        }

        public async Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime)
        {
            AccessToken = accessToken;
            _accessTokenExpirationTime = accessTokenExpirationTime;

            await _refreshTokenRepository.SetRefreshToken(refreshToken);
        }

        public async Task<bool> HasRefreshToken()
        {
            return !string.IsNullOrEmpty(await GetRefreshToken());
        }

        public async Task ClearRefreshToken()
        {
            await _refreshTokenRepository.ClearRefreshToken();
        }
    }
}