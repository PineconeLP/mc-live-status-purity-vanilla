namespace MCLiveStatus.PurityVanilla.Blazor.Models
{
    public class ServerStatusNotificationFactory
    {
        public Notification CreateJoinableExludingQueueNotification(string name, int onlinePlayers, int maxPlayersExcludingQueue)
        {
            return new Notification(
                $"{name} is now joinable!",
                $"{onlinePlayers} out of the max {maxPlayersExcludingQueue} players (excluding queue space) are online.");
        }

        public Notification CreateQueueJoinableNotification(string name, int onlinePlayers, int maxPlayers)
        {
            return new Notification(
                $"{name} queue is now joinable!",
                $"{onlinePlayers} out of the max {maxPlayers} players are online.");
        }

        public Notification CreateJoinableNotification(string name, int onlinePlayers, int maxPlayers)
        {
            return new Notification(
                $"{name} is now joinable!",
                $"{onlinePlayers} out of the max {maxPlayers} players are online.");
        }
    }
}