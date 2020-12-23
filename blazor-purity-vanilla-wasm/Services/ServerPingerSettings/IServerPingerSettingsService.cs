using System.Threading.Tasks;
using MCLiveStatus.ServerSettings.Domain.Models;
using Refit;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices
{
    public interface IServerPingerSettingsService
    {
        [Get("")]
        Task<ServerPingerSettings> GetSettings();

        [Post("")]
        Task SaveSettings([Body] ServerPingerSettings settings);
    }
}