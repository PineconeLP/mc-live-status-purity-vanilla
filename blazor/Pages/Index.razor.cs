using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using MCLiveStatus.Blazor.ViewModels;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.Blazor.Pages
{
    public partial class Index : ComponentBase, IDisposable
    {
        [Inject]
        public RepeatingServerPingerFactory RepeatingServerPingerFactory { get; set; }

        private ICollection<ServerListingItemViewModel> Servers { get; }

        private bool HasServers => Servers != null && Servers.Count() > 0;

        private ServerListingItemViewModel _purityVanillaServer;
        private RepeatingServerPinger _repeatingPinger;

        public Index()
        {
            Servers = new List<ServerListingItemViewModel>();
        }

        protected override async Task OnInitializedAsync()
        {
            ServerAddress serverAddress = new ServerAddress()
            {
                Host = "purityvanilla.com",
                Port = 25565
            };

            _purityVanillaServer = new ServerListingItemViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "Purity Vanilla",
                Description = "A fun vanilla server without hacks.",
                MaxPlayersExcludingQueue = 75
            };
            Servers.Add(_purityVanillaServer);

            _repeatingPinger = RepeatingServerPingerFactory.CreateRepeatingServerPinger(serverAddress);
            _repeatingPinger.PingCompleted += OnPingCompleted;
            await _repeatingPinger.Start(5);

            await base.OnInitializedAsync();
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            bool wasFullExcludingQueue = _purityVanillaServer.IsFullExcludingQueue;

            _purityVanillaServer.OnlinePlayers = response.OnlinePlayers;
            _purityVanillaServer.MaxPlayers = response.MaxPlayers;

            bool isFullExcludingQueue = _purityVanillaServer.IsFullExcludingQueue;

            if (wasFullExcludingQueue && !isFullExcludingQueue)
            {
                PushIsJoinableExcludingQueue(_purityVanillaServer);
            }

            InvokeAsync(StateHasChanged);
        }

        private void PushIsJoinableExcludingQueue(ServerListingItemViewModel server)
        {
            Electron.Notification.Show(new NotificationOptions(
                $"{server.Name} is now joinable!",
                $"{server.OnlinePlayers} out of the max {server.MaxPlayersExcludingQueue} players (excluding queue space) are online."));
        }

        public async void Dispose()
        {
            await _repeatingPinger.Stop();
        }
    }
}