using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers
{
    public interface IServerPinger
    {
        Task<ServerPingResponse> Ping();
    }
}