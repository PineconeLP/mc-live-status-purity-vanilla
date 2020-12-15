using System;
using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens
{
    public interface ITokenStore
    {
        string AccessToken { get; }
        bool IsAccessTokenExpired { get; }

        Task<bool> HasRefreshToken();
        Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);
    }
}