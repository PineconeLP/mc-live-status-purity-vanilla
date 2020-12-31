using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettingStores;
using Microsoft.AspNetCore.SignalR.Client;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers
{
    public class SignalRServerStatusPingerStore : IServerStatusPingerStore
    {
        private readonly ServerStatusPingerStoreState _state;
        private readonly ServerPingerSettingsStore _settingsStore;
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
            ServerPingerSettingsStore settingsStore,
            IServerPinger pinger,
            IServerStatusNotifier serverStatusNotifier,
            string negotiateUrl)
        {
            _state = state;
            _settingsStore = settingsStore;
            _pinger = pinger;
            _serverStatusNotifier = serverStatusNotifier;
            _negotiateUrl = negotiateUrl;

            _state.StateChanged += OnStateChanged;
            _settingsStore.SettingsChanged += UpdateHubConnection;

            _connection = new HubConnectionBuilder()
                .WithUrl(_negotiateUrl)
                .WithAutomaticReconnect()
                .Build();
            _connection.On<int, int>("ping", (online, max) => _state.OnNotificationPingCompleted(_serverStatusNotifier, online, max));
            _connection.On<int, string>("ping_failed", (code, message) => _state.OnPingFailed());
        }

        public async Task Load()
        {
            await LoadServerStatus();

            await _settingsStore.Load();

            await _serverStatusNotifier.RequestPermission();

            if (_settingsStore.AutoRefreshEnabled && _connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _connection.StartAsync();
                }
                catch (OperationCanceledException) { }
                catch (AggregateException) { }
                catch (Exception ex)
                {
                    _state.OnPingFailed(ex);
                }
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

        private async void UpdateHubConnection()
        {
            try
            {
                bool startConnectionRequested = _settingsStore.AutoRefreshEnabled;
                if (startConnectionRequested && _connection.State == HubConnectionState.Disconnected)
                {
                    await _connection.StartAsync();
                }
                else if (!startConnectionRequested && _connection.State != HubConnectionState.Disconnected)
                {
                    await _connection.StopAsync();
                }
            }
            catch (OperationCanceledException) { }
            catch (AggregateException) { }
        }

        public void Dispose()
        {
            _settingsStore.SettingsChanged -= UpdateHubConnection;
            _connection?.DisposeAsync();
            _state.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}