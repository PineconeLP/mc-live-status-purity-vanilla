using System;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MCLiveStatus.Blazor.ViewModels;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.Blazor.Components
{
    public partial class ServerLiveStatus : ComponentBase, IDisposable
    {
        [Inject]
        public RepeatingServerPingerFactory RepeatingServerPingerFactory { get; set; }

        [Parameter]
        public string Host { get; set; }

        [Parameter]
        public int Port { get; set; } = 25565;

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public int MaxPlayersExcludingQueue { get; set; }

        private int OnlinePlayers { get; set; }
        private int MaxPlayers { get; set; }
        private bool IsFull => OnlinePlayers >= MaxPlayers;
        private bool IsFullExcludingQueue => OnlinePlayers >= MaxPlayersExcludingQueue;

        private RepeatingServerPinger _repeatingPinger;

        protected override async Task OnInitializedAsync()
        {
            ServerAddress serverAddress = new ServerAddress(Host, Port);

            _repeatingPinger = RepeatingServerPingerFactory.CreateRepeatingServerPinger(serverAddress);
            _repeatingPinger.PingCompleted += OnPingCompleted;
            await _repeatingPinger.Start(5);

            await base.OnInitializedAsync();
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            bool wasFullExcludingQueue = IsFullExcludingQueue;

            OnlinePlayers = response.OnlinePlayers;
            MaxPlayers = response.MaxPlayers;

            bool isFullExcludingQueue = IsFullExcludingQueue;

            if (wasFullExcludingQueue && !isFullExcludingQueue)
            {
                PushIsJoinableExcludingQueue();
            }

            InvokeAsync(StateHasChanged);
        }

        private void PushIsJoinableExcludingQueue()
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{Name} is now joinable!",
                $"{OnlinePlayers} out of the max {MaxPlayersExcludingQueue} players (excluding queue space) are online."));
        }

        public async void Dispose()
        {
            await _repeatingPinger?.Stop();
        }
    }
}