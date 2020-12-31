using System;
using System.Threading.Tasks;
using Endpointer.Core.Client.Exceptions;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores
{
    public interface IServerPingerSettingsStore
    {
        bool AllowNotifyJoinable { get; set; }
        bool AllowNotifyQueueJoinable { get; set; }
        bool HasDirtySettings { get; }
        bool IsLoading { get; }

        event Action SettingsChanged;
        event Action LoadRequested;
        event Action HasDirtySettingsChanged;
        event Action IsLoadingChanged;

        /// <summary>
        /// Load server pinger settings for the current user.
        /// </summary>
        /// <exception cref="UnauthorizedException">Thrown if authentication fails.</exception>
        /// <exception cref="Exception">Thrown if load fails.</exception>
        Task Load();

        /// <summary>
        /// Save server pinger settings.
        /// </summary>
        /// <exception cref="UnauthorizedException">Thrown if authentication fails.</exception>
        /// <exception cref="Exception">Thrown if save fails.</exception>
        Task Save();
    }
}