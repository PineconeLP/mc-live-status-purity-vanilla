using System;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores
{
    public interface IPingConfigurableServerPingerSettingsStore : IServerPingerSettingsStore
    {
        double PingIntervalSeconds { get; set; }
        bool IsInvalidPingIntervalSeconds { get; set; }

        event Action ValidationChanged;
    }
}