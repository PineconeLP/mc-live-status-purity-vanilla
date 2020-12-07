using MCLiveStatus.PurityVanilla.Blazor.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers.NotificationPermitters
{
    public interface IServerStatusNotificationPermitter
    {
        bool CanNotifyJoinableExludingQueue(IPingedServerDetails serverDetails, bool wasFullExcludingQueue);
        bool CanNotifyQueueJoinable(IPingedServerDetails serverDetails, bool wasFull);
        bool CanNotifyJoinable(IPingedServerDetails serverDetails, bool wasFull);
    }
}