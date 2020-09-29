using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Description = "A fun vanilla server without hacks."
            };
            Servers.Add(_purityVanillaServer);

            _repeatingPinger = RepeatingServerPingerFactory.CreateRepeatingServerPinger(serverAddress);
            _repeatingPinger.PingCompleted += OnPingCompleted;
            await _repeatingPinger.Start(5);

            await base.OnInitializedAsync();
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            _purityVanillaServer.OnlinePlayers = response.OnlinePlayers;
            _purityVanillaServer.MaxPlayers = response.MaxPlayers;
            InvokeAsync(StateHasChanged);
        }

        public async void Dispose()
        {
            await _repeatingPinger.Stop();
        }
    }
}