using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers
{
    public class ServerStatusPingerStore : IServerStatusPingerStore
    {
        public IPingedServerDetails ServerDetails => new PingedServerDetails(new ServerDetails());

        public DateTime LastUpdateTime => DateTime.Now;

        public bool HasUpdateError => false;

        public DateTime LastUpdateErrorTime => DateTime.Now;

        public event Action StateChanged;

        public void Dispose()
        {

        }

        public async Task Load()
        {

        }

        public async Task RefreshServerStatus()
        {

        }
    }
}