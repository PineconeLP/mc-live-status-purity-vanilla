using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers
{
    public interface IServerStatusPingerStore
    {
        IPingedServerDetails ServerDetails { get; }
        DateTime LastUpdateTime { get; }
        bool HasUpdateError { get; }
        DateTime LastUpdateErrorTime { get; }

        event Action StateChanged;

        void Dispose();
        Task Load();
        Task RefreshServerStatus();
    }
}