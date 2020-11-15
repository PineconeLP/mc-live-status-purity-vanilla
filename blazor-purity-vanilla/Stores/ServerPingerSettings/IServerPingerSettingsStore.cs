using System;
using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores
{
    public interface IServerPingerSettingsStore
    {
        bool AllowNotifyJoinable { get; set; }
        bool AllowNotifyQueueJoinable { get; set; }
        double PingIntervalSeconds { get; set; }
        bool IsInvalidPingIntervalSeconds { get; set; }
        bool IsLoading { get; }

        event Action SettingsChanged;
        event Action ValidationChanged;
        event Action IsLoadingChanged;

        Task Load();
        Task Save();
    }
}