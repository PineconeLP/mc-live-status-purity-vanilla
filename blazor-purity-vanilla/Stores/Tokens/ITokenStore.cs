using System;
using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens
{
    public interface ITokenStore
    {
        string AccessToken { get; }
        bool IsAccessTokenExpired { get; }

        Task<bool> HasRefreshToken();
        Task<string> GetRefreshToken();

        Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);

        Task ClearRefreshToken();
    }
}