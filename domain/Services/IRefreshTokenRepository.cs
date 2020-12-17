using System.Threading.Tasks;

namespace MCLiveStatus.Domain.Services
{
    public interface IRefreshTokenRepository
    {
        Task<string> GetRefreshToken();
        Task SetRefreshToken(string refreshToken);
        Task ClearRefreshToken();
    }
}