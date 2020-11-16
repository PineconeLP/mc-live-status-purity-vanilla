using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using Microsoft.AspNetCore.SignalR.Client;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers
{
    public class SignalRServerStatusPingerStore : IServerStatusPingerStore
    {
        private HubConnection _connection;
        private readonly PingedServerDetails _serverDetails;

        public IPingedServerDetails ServerDetails => _serverDetails;

        public DateTime LastUpdateTime { get; private set; }

        public bool HasUpdateError { get; private set; }

        public DateTime LastUpdateErrorTime { get; private set; }

        public event Action StateChanged;

        public SignalRServerStatusPingerStore()
        {
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
            _connection = new HubConnectionBuilder()
                .WithUrl("http://0.0.0.0:7071/api")
                .Build();

            _connection.On<int, int>("ping", OnPing);

            await _connection.StartAsync();
        }

        private void OnPing(int online, int max)
        {
            HasUpdateError = false;
            LastUpdateTime = DateTime.Now;

            _serverDetails.HasData = true;
            _serverDetails.OnlinePlayers = online;
            _serverDetails.MaxPlayers = max;

            OnStateChanged();
        }

        public async Task RefreshServerStatus()
        {

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