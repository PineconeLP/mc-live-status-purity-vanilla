using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using Microsoft.AspNetCore.SignalR.Client;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers
{
    public class SignalRServerStatusPingerStore : IServerStatusPingerStore
    {
        private readonly IServerPinger _pinger;
        private readonly string _negotiateUrl;
        private readonly PingedServerDetails _serverDetails;

        private HubConnection _connection;

        public IPingedServerDetails ServerDetails => _serverDetails;

        public DateTime LastUpdateTime { get; private set; }

        public bool HasUpdateError { get; private set; }

        public DateTime LastUpdateErrorTime { get; private set; }

        public event Action StateChanged;

        public SignalRServerStatusPingerStore(IServerPinger pinger, string negotiateUrl)
        {
            _pinger = pinger;
            _negotiateUrl = negotiateUrl;

            _serverDetails = new PingedServerDetails(new ServerDetails()
            {
                Name = "Purity Vanilla",
                Host = "purityvanilla.com",
                Port = 25565,
                HasQueue = true,
                MaxPlayersExcludingQueue = 75
            });
        }

        public async Task Load()
        {
            await RefreshServerStatus();

            _connection = new HubConnectionBuilder()
                .WithUrl(_negotiateUrl)
                .Build();

            _connection.On<int, int>("ping", OnPingCompleted);

            await _connection.StartAsync();
        }

        public async Task RefreshServerStatus()
        {
            try
            {
                ServerPingResponse response = await _pinger.Ping();
                OnPingCompleted(response.OnlinePlayers, response.MaxPlayers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                OnPingFailed(ex);
            }
        }

        private void OnPingCompleted(int online, int max)
        {
            HasUpdateError = false;
            LastUpdateTime = DateTime.Now;

            _serverDetails.HasData = true;
            _serverDetails.OnlinePlayers = online;
            _serverDetails.MaxPlayers = max;

            OnStateChanged();
        }

        private void OnPingFailed(Exception ex)
        {
            HasUpdateError = true;
            LastUpdateErrorTime = DateTime.Now;
            OnStateChanged();
        }

        public void Dispose()
        {
            _connection?.DisposeAsync();
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}