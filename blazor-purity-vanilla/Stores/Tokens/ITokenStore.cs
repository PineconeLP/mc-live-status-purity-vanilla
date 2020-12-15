using System;
using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens
{
    public interface ITokenStore
    {
        string AccessToken { get; }

        Task SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpirationTime);
    }
}