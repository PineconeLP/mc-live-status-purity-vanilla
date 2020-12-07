using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.ServerStatusPingers
{
    public class ServerStatusPingerStore : IDisposable, IServerStatusPingerStore
    {
        private readonly ServerStatusPingerStoreState _state;
        private readonly ServerAddress _serverAddress;
        private readonly IServerPinger _pinger;
        private readonly RepeatingServerPinger _repeatingPinger;
        private readonly IServerPingerSettingsStore _settingsStore;
        private readonly IServerStatusNotifier _serverStatusNotifier;

        public IPingedServerDetails ServerDetails => _state.ServerDetails;
        public DateTime LastUpdateTime => _state.LastUpdateTime;
        public bool HasUpdateError => _state.HasUpdateError;
        public DateTime LastUpdateErrorTime => _state.LastUpdateErrorTime;

        public event Action StateChanged;

        public ServerStatusPingerStore(ServerStatusPingerStoreState state,
            ServerDetails serverDetails,
            IServerPinger pinger,
            RepeatingServerPingerFactory repeatingServerPingerFactory,
            IServerPingerSettingsStore settingsStore,
            IServerStatusNotifier serverStatusNotifier)
        {
            _state = state;
            _serverAddress = new ServerAddress(ServerDetails.Host, ServerDetails.Port);
            _pinger = pinger;
            _repeatingPinger = repeatingServerPingerFactory.CreateRepeatingServerPinger(_serverAddress);
            _settingsStore = settingsStore;
            _serverStatusNotifier = serverStatusNotifier;

            _state.StateChanged += OnStateChanged;
            _repeatingPinger.PingCompleted += OnNotificationPingCompleted;
            _repeatingPinger.PingFailed += _state.OnPingFailed;
            _settingsStore.SettingsChanged += UpdatePingInterval;
        }

        public async Task Load()
        {
            await _settingsStore.Load();

            await LoadServerStatus();

            try
            {
                await _repeatingPinger.Start(_settingsStore.PingIntervalSeconds);
            }
            catch (ArgumentException)
            {
                _settingsStore.IsInvalidPingIntervalSeconds = true;
            }
        }

        private async Task LoadServerStatus()
        {
            try
            {
                ServerPingResponse initialResponse = await _pinger.Ping(_serverAddress);
                OnPingCompleted(initialResponse);
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
                ServerPingResponse initialResponse = await _pinger.Ping(_serverAddress);
                OnPingCompleted(initialResponse);
            }
            catch (Exception ex)
            {
                _state.OnPingFailed(ex);
            }
        }

        private void OnNotificationPingCompleted(ServerPingResponse response)
        {
            _state.OnNotificationPingCompleted(_serverStatusNotifier, response.OnlinePlayers, response.MaxPlayers);
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            _state.OnPingCompleted(response.OnlinePlayers, response.MaxPlayers);
        }

        private async void UpdatePingInterval()
        {
            _settingsStore.IsInvalidPingIntervalSeconds = false;

            try
            {
                if (!_repeatingPinger.IsRunning)
                {
                    await _repeatingPinger.Start(_settingsStore.PingIntervalSeconds);
                }
                else
                {
                    await _repeatingPinger.UpdateServerPingSecondsInterval(_settingsStore.PingIntervalSeconds);
                }
            }
            catch (ArgumentException)
            {
                _settingsStore.IsInvalidPingIntervalSeconds = true;
            }
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }

        public async void Dispose()
        {
            await _repeatingPinger.Stop();

            _state.StateChanged -= OnStateChanged;
            _repeatingPinger.PingCompleted -= OnNotificationPingCompleted;
            _repeatingPinger.PingFailed -= _state.OnPingFailed;
            _settingsStore.SettingsChanged -= UpdatePingInterval;
        }
    }
}