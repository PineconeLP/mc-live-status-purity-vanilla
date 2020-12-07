using System;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers.NotificationPermitters;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers
{
    public class ServerStatusPingerStoreState
    {
        private readonly PingedServerDetails _serverDetails;
        private readonly IServerStatusNotificationPermitter _notificationPermitter;

        public IPingedServerDetails ServerDetails => _serverDetails;
        public DateTime LastUpdateTime { get; private set; }
        public bool HasUpdateError { get; private set; }
        public DateTime LastUpdateErrorTime { get; private set; }

        public event Action StateChanged;

        public ServerStatusPingerStoreState(ServerDetails serverDetails, IServerStatusNotificationPermitter notificationPermitter)
        {
            _serverDetails = new PingedServerDetails(serverDetails);
            _notificationPermitter = notificationPermitter;
        }

        public void OnNotificationPingCompleted(IServerStatusNotifier serverStatusNotifier, int online, int max)
        {
            bool wasFull = _serverDetails.IsFull;
            bool wasFullExcludingQueue = _serverDetails.IsFullExcludingQueue;

            OnPingCompleted(online, max);

            TryNotify(serverStatusNotifier, wasFull, wasFullExcludingQueue);
        }

        public void OnPingCompleted(int online, int max)
        {
            HasUpdateError = false;
            LastUpdateTime = DateTime.Now;

            _serverDetails.HasData = true;
            _serverDetails.OnlinePlayers = online;
            _serverDetails.MaxPlayers = max;

            OnStateChanged();
        }

        public void OnPingFailed(Exception ex)
        {
            HasUpdateError = true;
            LastUpdateErrorTime = DateTime.Now;
            OnStateChanged();
        }

        private void TryNotify(IServerStatusNotifier serverStatusNotifier, bool wasFull, bool wasFullExcludingQueue)
        {
            if (ServerDetails.HasQueue)
            {
                if (_notificationPermitter.CanNotifyJoinableExludingQueue(ServerDetails, wasFullExcludingQueue))
                {
                    serverStatusNotifier.NotifyJoinableExludingQueue(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayersExcludingQueue);
                }
                else if (_notificationPermitter.CanNotifyQueueJoinable(ServerDetails, wasFull))
                {
                    serverStatusNotifier.NotifyQueueJoinable(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayers);
                }
            }
            else if (_notificationPermitter.CanNotifyJoinable(ServerDetails, wasFull))
            {
                serverStatusNotifier.NotifyJoinable(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayers);
            }
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}