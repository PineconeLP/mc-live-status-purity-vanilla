using ElectronNET.API;
using ElectronNET.API.Entities;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.ServerStatusNotifiers
{
    public class ElectronServerStatusNotifier : IServerStatusNotifier
    {
        public void NotifyJoinableExludingQueue(string name, int onlinePlayers, int maxPlayersExcludingQueue)
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{name} is now joinable!",
                $"{onlinePlayers} out of the max {maxPlayersExcludingQueue} players (excluding queue space) are online."));
        }

        public void NotifyQueueJoinable(string name, int onlinePlayers, int maxPlayers)
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{name} queue is now joinable!",
                $"{onlinePlayers} out of the max {maxPlayers} players are online."));
        }

        public void NotifyJoinable(string name, int onlinePlayers, int maxPlayers)
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{name} is now joinable!",
                $"{onlinePlayers} out of the max {maxPlayers} players are online."));
        }
    }
}