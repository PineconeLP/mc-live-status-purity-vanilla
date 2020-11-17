using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers
{
    public interface IServerStatusNotifier
    {
        void NotifyJoinable(string name, int onlinePlayers, int maxPlayers);
        void NotifyJoinableExludingQueue(string name, int onlinePlayers, int maxPlayersExcludingQueue);
        void NotifyQueueJoinable(string name, int onlinePlayers, int maxPlayers);
        Task RequestPermission();
    }
}