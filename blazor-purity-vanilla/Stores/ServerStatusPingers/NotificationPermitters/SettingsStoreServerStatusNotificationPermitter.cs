using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers.NotificationPermitters
{
    public class SettingsStoreServerStatusNotificationPermitter : IServerStatusNotificationPermitter
    {
        private readonly ServerStatusNotificationPermitter _baseNotificationPermitter;
        private readonly IServerPingerSettingsStore _settingsStore;

        public SettingsStoreServerStatusNotificationPermitter(ServerStatusNotificationPermitter baseNotificationPermitter,
            IServerPingerSettingsStore settingsStore)
        {
            _baseNotificationPermitter = baseNotificationPermitter;
            _settingsStore = settingsStore;
        }

        public bool CanNotifyJoinable(IPingedServerDetails serverDetails, bool wasFull)
        {
            return _baseNotificationPermitter.CanNotifyJoinable(serverDetails, wasFull) && _settingsStore.AllowNotifyJoinable;
        }

        public bool CanNotifyJoinableExludingQueue(IPingedServerDetails serverDetails, bool wasFullExcludingQueue)
        {
            return _baseNotificationPermitter.CanNotifyJoinableExludingQueue(serverDetails, wasFullExcludingQueue) && _settingsStore.AllowNotifyJoinable;
        }

        public bool CanNotifyQueueJoinable(IPingedServerDetails serverDetails, bool wasFull)
        {
            return _baseNotificationPermitter.CanNotifyQueueJoinable(serverDetails, wasFull) && _settingsStore.AllowNotifyQueueJoinable;
        }
    }
}