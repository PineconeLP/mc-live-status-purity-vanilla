using System;
using System.Threading.Tasks;
using Endpointer.Core.Client.Stores;
using MCLiveStatus.Domain.Services;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.Tokens
{
    public class TokenStore : AutoRefreshTokenStoreBase, ITokenStore
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        private string _accessToken;

        public override string AccessToken => _accessToken;

        public TokenStore(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public override async Task<string> GetRefreshToken()
        {
            return await _refreshTokenRepository.GetRefreshToken();
        }

        public override async Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime)
        {
            _accessToken = accessToken;
            AccessTokenExpirationTime = accessTokenExpirationTime;

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