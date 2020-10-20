using System;
using System.Globalization;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components
{
    public partial class ServerLiveStatus : ComponentBase, IDisposable
    {
        [Inject]
        public RepeatingServerPingerFactory RepeatingServerPingerFactory { get; set; }

        [Parameter]
        public string Host { get; set; }

        [Parameter]
        public int Port { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public bool HasQueue { get; set; }

        [Parameter]
        public int MaxPlayersExcludingQueue { get; set; }

        private int OnlinePlayers { get; set; }
        private int MaxPlayers { get; set; }
        private bool IsFull => OnlinePlayers >= MaxPlayers;
        private bool IsFullExcludingQueue => HasQueue ? OnlinePlayers >= MaxPlayersExcludingQueue : IsFull;
        private bool IsLoading { get; set; }
        private DateTime LastUpdateTime { get; set; }
        private string LastUpdateTimeDisplay => ToDisplayString(LastUpdateTime);
        private bool HasUpdateError { get; set; }
        private DateTime LastUpdateErrorTime { get; set; }
        private string LastUpdateErrorTimeDisplay => ToDisplayString(LastUpdateErrorTime);

        private bool AllowNotifyJoinable { get; set; }
        private bool AllowNotifyQueueJoinable { get; set; }
        private double ServerStatusPingIntervalSeconds { get; set; }
        private bool IsInvalidServerStatusPingIntervalSeconds { get; set; }
        private string InvalidServerStatusPingIntervalSecondsClass => IsInvalidServerStatusPingIntervalSeconds ? "is-invalid" : "";

        private RepeatingServerPinger _repeatingPinger;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            ServerStatusPingIntervalSeconds = 5;
            ServerAddress serverAddress = new ServerAddress(Host, Port);

            _repeatingPinger = RepeatingServerPingerFactory.CreateRepeatingServerPinger(serverAddress);
            _repeatingPinger.PingCompleted += OnPingCompleted;
            _repeatingPinger.PingFailed += OnPingFailed;
            await _repeatingPinger.Start(ServerStatusPingIntervalSeconds);

            await base.OnInitializedAsync();
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            ClearUpdateError();
            IsLoading = false;

            LastUpdateTime = DateTime.Now;

            bool wasFull = IsFull;
            bool wasFullExcludingQueue = IsFullExcludingQueue;

            OnlinePlayers = response.OnlinePlayers;
            MaxPlayers = response.MaxPlayers;

            TryNotify(wasFull, wasFullExcludingQueue);

            InvokeAsync(StateHasChanged);
        }

        private void OnPingFailed(Exception ex)
        {
            HasUpdateError = true;
            LastUpdateErrorTime = DateTime.Now;
            InvokeAsync(StateHasChanged);
        }

        private void TryNotify(bool wasFull, bool wasFullExcludingQueue)
        {
            if (HasQueue)
            {
                if (AllowNotifyJoinable && wasFullExcludingQueue && !IsFullExcludingQueue)
                {
                    NotifyJoinableExludingQueue();
                }
                else if (AllowNotifyQueueJoinable && wasFull && !IsFull)
                {
                    NotifyQueueJoinable();
                }
            }
            else
            {
                if (AllowNotifyJoinable && wasFull && !IsFull)
                {
                    NotifyJoinable();
                }
            }
        }

        private void NotifyJoinableExludingQueue()
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{Name} is now joinable!",
                $"{OnlinePlayers} out of the max {MaxPlayersExcludingQueue} players (excluding queue space) are online."));
        }

        private void NotifyQueueJoinable()
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{Name} queue is now joinable!",
                $"{OnlinePlayers} out of the max {MaxPlayers} players are online."));
        }

        private void NotifyJoinable()
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{Name} is now joinable!",
                $"{OnlinePlayers} out of the max {MaxPlayers} players are online."));
        }

        private async Task UpdatePingInterval()
        {
            IsInvalidServerStatusPingIntervalSeconds = false;

            try
            {
                await _repeatingPinger.UpdateServerPingSecondsInterval(ServerStatusPingIntervalSeconds);
            }
            catch (ArgumentException)
            {
                IsInvalidServerStatusPingIntervalSeconds = true;
            }
        }

        private void ClearUpdateError()
        {
            HasUpdateError = false;
        }

        private string ToDisplayString(DateTime dateTime)
        {
            return dateTime.ToString("MMM d 'at' hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        public async void Dispose()
        {
            if (_repeatingPinger != null)
            {
                _repeatingPinger.PingCompleted -= OnPingCompleted;
                _repeatingPinger.PingFailed -= OnPingFailed;
                await _repeatingPinger?.Stop();
            }
        }
    }
}