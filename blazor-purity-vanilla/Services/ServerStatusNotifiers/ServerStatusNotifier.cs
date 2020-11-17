
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.Notifiers;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.ServerStatusNotifiers
{
    public class ServerStatusNotifier : IServerStatusNotifier
    {
        private readonly INotifier _notifier;
        private readonly ServerStatusNotificationFactory _notificationFactory;

        public ServerStatusNotifier(INotifier notifier, ServerStatusNotificationFactory notificationFactory)
        {
            _notifier = notifier;
            _notificationFactory = notificationFactory;
        }

        public void NotifyJoinableExludingQueue(string name, int onlinePlayers, int maxPlayersExcludingQueue)
        {
            Notification notification = _notificationFactory.CreateJoinableExludingQueueNotification(name, onlinePlayers, maxPlayersExcludingQueue);

            _notifier.Show(notification);
        }

        public void NotifyQueueJoinable(string name, int onlinePlayers, int maxPlayers)
        {
            Notification notification = _notificationFactory.CreateQueueJoinableNotification(name, onlinePlayers, maxPlayers);

            _notifier.Show(notification);
        }

        public void NotifyJoinable(string name, int onlinePlayers, int maxPlayers)
        {
            Notification notification = _notificationFactory.CreateJoinableNotification(name, onlinePlayers, maxPlayers);

            _notifier.Show(notification);
        }

        public Task RequestPermission()
        {
            return _notifier.RequestPermission();
        }
    }
}