using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettingStores;
using Microsoft.AspNetCore.SignalR.Client;
using Polly;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers
{
    public class SignalRServerStatusPingerStore : IServerStatusPingerStore
    {
        private readonly ServerStatusPingerStoreState _state;
        private readonly ServerPingerSettingsStore _settingsStore;
        private readonly IServerPinger _pinger;
        private readonly IServerStatusNotifier _serverStatusNotifier;
        private readonly string _negotiateUrl;

        private readonly AsyncPolicy _initalServerPingRetryPolicy;

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

            _initalServerPingRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(retryNumber => TimeSpan.FromSeconds(5));

            _state.StateChanged += OnStateChanged;
            _settingsStore.SettingsChanged += HandleSettingsChanged;

            _connection = new HubConnectionBuilder()
                .WithUrl(_negotiateUrl)
                .WithAutomaticReconnect()
                .Build();
            _connection.On<int, int>("ping", (online, max) => _state.OnNotificationPingCompleted(_serverStatusNotifier, online, max));
            _connection.On<int, string>("ping_failed", (code, message) => _state.OnPingFailed());
        }

        public async Task Load()
        {
            try
            {
                await _settingsStore.Load();
            }
            catch (Exception) { } // Ignore settings store load exceptions. Throw exception at end of this method?

            await _serverStatusNotifier.RequestPermission();

            await _initalServerPingRetryPolicy.ExecuteAsync(LoadInitialServerStatus);

            await UpdateHubConnection();
        }

        /// <summary>
        /// Load the initial server ping status.
        /// </summary>
        /// <exception cref="Exception">Thrown if ping fails.</exception>
        private async Task LoadInitialServerStatus()
        {
            try
            {
                ServerPingResponse response = await _pinger.Ping();
                _state.OnPingCompleted(response.OnlinePlayers, response.MaxPlayers);
            }
            catch (Exception ex)
            {
                _state.OnPingFailed(ex);
                throw;
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

        private async Task UpdateHubConnection()
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
            // catch (OperationCanceledException) { }
            // catch (AggregateException) { }
            catch (Exception ex)
            {
                _state.OnPingFailed(ex);
                // _state.OnPingerConnectionFailed(ex)?
            } // Yuck, this needs to be cleaned up somehow, retries?
        }

        private async void HandleSettingsChanged()
        {
            await UpdateHubConnection();
        }

        public void Dispose()
        {
            _settingsStore.SettingsChanged -= HandleSettingsChanged;
            _connection?.DisposeAsync();
            _state.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}