using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using Microsoft.AspNetCore.SignalR.Client;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers
{
    public class SignalRServerStatusPingerStore : IServerStatusPingerStore
    {
        private readonly ServerStatusPingerStoreState _state;
        private readonly IServerPinger _pinger;
        private readonly IServerStatusNotifier _serverStatusNotifier;
        private readonly string _negotiateUrl;

        private HubConnection _connection;

        public IPingedServerDetails ServerDetails => _state.ServerDetails;
        public DateTime LastUpdateTime => _state.LastUpdateTime;
        public bool HasUpdateError => _state.HasUpdateError;
        public DateTime LastUpdateErrorTime => _state.LastUpdateErrorTime;

        public event Action StateChanged;

        public SignalRServerStatusPingerStore(ServerStatusPingerStoreState state,
            IServerPinger pinger,
            IServerStatusNotifier serverStatusNotifier,
            string negotiateUrl)
        {
            _state = state;
            _pinger = pinger;
            _serverStatusNotifier = serverStatusNotifier;
            _negotiateUrl = negotiateUrl;

            _state.StateChanged += OnStateChanged;
        }

        public async Task Load()
        {
            await LoadServerStatus();

            await _serverStatusNotifier.RequestPermission();

            _connection = new HubConnectionBuilder()
                .WithUrl(_negotiateUrl)
                .Build();

            _connection.On<int, int>("ping", (online, max) => _state.OnNotificationPingCompleted(_serverStatusNotifier, online, max));

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                _state.OnPingFailed(ex);
            }
        }

        private async Task LoadServerStatus()
        {
            try
            {
                ServerPingResponse response = await _pinger.Ping();
                _state.OnPingCompleted(response.OnlinePlayers, response.MaxPlayers);
            }
            catch (Exception ex)
            {
                _state.OnPingFailed(ex);
            }
        }

        public async Task RefreshServerStatus()
        {
            try
            {
                ServerPingResponse response = await _pinger.Ping();
                _state.OnNotificationPingCompleted(_serverStatusNotifier, response.OnlinePlayers, response.MaxPlayers);
            }
            catch (Exception ex)
            {
                _state.OnPingFailed(ex);
            }
        }

        public void Dispose()
        {
            _connection?.DisposeAsync();
            _state.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}