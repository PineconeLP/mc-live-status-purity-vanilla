using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers
{
    public class ServerStatusPingerStore : IDisposable
    {
        private readonly PingedServerDetails _serverDetails;
        private readonly RepeatingServerPinger _repeatingPinger;
        private readonly ServerPingerSettingsStore _settingsStore;
        private readonly IServerStatusNotifier _serverStatusNotifier;

        private bool _isInitialized;

        public IPingedServerDetails ServerDetails => _serverDetails;
        public bool IsLoading { get; private set; }
        public DateTime LastUpdateTime { get; private set; }
        public bool HasUpdateError { get; private set; }
        public DateTime LastUpdateErrorTime { get; private set; }

        public event Action StateChanged;

        public ServerStatusPingerStore(ServerDetails serverDetails,
            RepeatingServerPingerFactory repeatingServerPingerFactory,
            ServerPingerSettingsStore settingsStore,
            IServerStatusNotifier serverStatusNotifier)
        {
            _serverDetails = new PingedServerDetails(serverDetails);

            ServerAddress serverAddress = new ServerAddress(_serverDetails.Host, _serverDetails.Port);
            _repeatingPinger = repeatingServerPingerFactory.CreateRepeatingServerPinger(serverAddress);
            _settingsStore = settingsStore;
            _serverStatusNotifier = serverStatusNotifier;

            _repeatingPinger.PingCompleted += OnPingCompleted;
            _repeatingPinger.PingFailed += OnPingFailed;
            _settingsStore.SettingsChanged += UpdatePingInterval;
        }

        public async Task Initialize()
        {
            if (!_isInitialized && !IsLoading)
            {
                IsLoading = true;
                OnStateChanged();

                await _repeatingPinger.Start(_settingsStore.PingIntervalSeconds);

                _isInitialized = true;
            }
        }

        private void OnPingCompleted(ServerPingResponse response)
        {
            HasUpdateError = false;
            IsLoading = false;

            LastUpdateTime = DateTime.Now;

            bool wasFull = _serverDetails.IsFull;
            bool wasFullExcludingQueue = _serverDetails.IsFullExcludingQueue;

            _serverDetails.OnlinePlayers = response.OnlinePlayers;
            _serverDetails.MaxPlayers = response.MaxPlayers;

            OnStateChanged();

            TryNotify(wasFull, wasFullExcludingQueue);
        }

        private void OnPingFailed(Exception ex)
        {
            HasUpdateError = true;
            LastUpdateErrorTime = DateTime.Now;
            OnStateChanged();
        }

        private void TryNotify(bool wasFull, bool wasFullExcludingQueue)
        {
            if (ServerDetails.HasQueue)
            {
                if (_settingsStore.AllowNotifyJoinable && wasFullExcludingQueue && !ServerDetails.IsFullExcludingQueue)
                {
                    _serverStatusNotifier.NotifyJoinableExludingQueue(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayersExcludingQueue);
                }
                else if (_settingsStore.AllowNotifyQueueJoinable && wasFull && !ServerDetails.IsFull)
                {
                    _serverStatusNotifier.NotifyQueueJoinable(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayers);
                }
            }
            else
            {
                if (_settingsStore.AllowNotifyJoinable && wasFull && !ServerDetails.IsFull)
                {
                    _serverStatusNotifier.NotifyJoinable(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayers);
                }
            }
        }

        private async void UpdatePingInterval()
        {
            _settingsStore.IsInvalidPingIntervalSeconds = false;

            try
            {
                await _repeatingPinger.UpdateServerPingSecondsInterval(_settingsStore.PingIntervalSeconds);
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

            _repeatingPinger.PingCompleted -= OnPingCompleted;
            _repeatingPinger.PingFailed -= OnPingFailed;
            _settingsStore.SettingsChanged -= UpdatePingInterval;
        }
    }
}