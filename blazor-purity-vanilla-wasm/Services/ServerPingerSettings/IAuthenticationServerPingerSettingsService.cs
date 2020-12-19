using System.Threading.Tasks;
using MCLiveStatus.ServerSettings.Domain.Models;
using Refit;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices
{
    public interface IAuthenticationServerPingerSettingsService
    {
        [Get("")]
        Task<ServerPingerSettings> GetSettings([Header("Authorization")] string token);

        [Post("")]
        Task SaveSettings([Header("Authorization")] string token, [Body] ServerPingerSettings settings);
    }
}