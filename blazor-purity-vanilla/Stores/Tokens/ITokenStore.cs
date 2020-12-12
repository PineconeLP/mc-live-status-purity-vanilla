using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens
{
    public interface ITokenStore
    {
        string AccessToken { get; set; }

        Task<string> GetRefreshToken();
        Task SetRefreshToken(string refreshToken);

        void ClearAccessToken();
        Task ClearRefreshToken();
    }
}