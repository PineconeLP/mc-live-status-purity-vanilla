using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettings
{
    public class ServerPingerSettingsStore : IAutoRefreshServerPingerSettingsStore
    {
        private readonly IServerPingerSettingsService _settingsService;

        private bool _autoRefreshEnabled;
        private bool _allowNotifyJoinable;
        private bool _allowNotifyQueueJoinable;
        private bool _isLoading;

        public bool AutoRefreshEnabled
        {
            get => _autoRefreshEnabled;
            set
            {
                _autoRefreshEnabled = value;
                OnSettingsChanged();
            }
        }

        public bool AllowNotifyJoinable
        {
            get => _allowNotifyJoinable;
            set
            {
                _allowNotifyJoinable = value;
                OnSettingsChanged();
            }
        }

        public bool AllowNotifyQueueJoinable
        {
            get => _allowNotifyQueueJoinable;
            set
            {
                _allowNotifyQueueJoinable = value;
                OnSettingsChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnIsLoadingChanged();
            }
        }

        public event Action SettingsChanged;
        public event Action IsLoadingChanged;

        public ServerPingerSettingsStore(IServerPingerSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task Load()
        {
            throw new NotImplementedException();
        }

        public async Task Save()
        {
            throw new NotImplementedException();
        }

        private void OnIsLoadingChanged()
        {
            IsLoadingChanged?.Invoke();
        }

        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke();
        }
    }
}