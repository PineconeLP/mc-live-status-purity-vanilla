using System.Threading.Tasks;
using MCLiveStatus.ServerSettings.Domain.Models;
using Refit;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices
{
    public interface IServerPingerSettingsService
    {
        Task<ServerPingerSettings> GetSettings();

        Task SaveSettings(ServerPingerSettings settings);
    }
}