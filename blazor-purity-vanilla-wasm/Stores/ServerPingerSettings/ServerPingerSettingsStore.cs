using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettings
{
    public class ServerPingerSettingsStore : IServerPingerSettingsStore
    {
        public bool AllowNotifyJoinable { get; set; }
        public bool AllowNotifyQueueJoinable { get; set; }
        public double PingIntervalSeconds { get; set; }
        public bool IsInvalidPingIntervalSeconds { get; set; }

        public bool IsLoading => false;

        public event Action SettingsChanged;
        public event Action ValidationChanged;
        public event Action IsLoadingChanged;

        public async Task Load()
        {

        }

        public async Task Save()
        {

        }
    }
}