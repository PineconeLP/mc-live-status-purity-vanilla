using MCLiveStatus.PurityVanilla.Blazor.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers.NotificationPermitters
{
    public class ServerStatusNotificationPermitter : IServerStatusNotificationPermitter
    {
        public bool CanNotifyJoinable(IPingedServerDetails serverDetails, bool wasFull)
        {
            return wasFull && !serverDetails.IsFull;
        }

        public bool CanNotifyJoinableExludingQueue(IPingedServerDetails serverDetails, bool wasFullExcludingQueue)
        {
            return wasFullExcludingQueue && !serverDetails.IsFullExcludingQueue;
        }

        public bool CanNotifyQueueJoinable(IPingedServerDetails serverDetails, bool wasFull)
        {
            return wasFull && !serverDetails.IsFull;
        }
    }
}