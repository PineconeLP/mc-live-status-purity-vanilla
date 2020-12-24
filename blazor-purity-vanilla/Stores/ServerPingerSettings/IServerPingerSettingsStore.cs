using System;
using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores
{
    public interface IServerPingerSettingsStore
    {
        bool AllowNotifyJoinable { get; set; }
        bool AllowNotifyQueueJoinable { get; set; }
        bool HasDirtySettings { get; }
        bool IsLoading { get; }

        event Action SettingsChanged;
        event Action HasDirtySettingsChanged;
        event Action IsLoadingChanged;

        Task Load();
        Task Save();
    }
}