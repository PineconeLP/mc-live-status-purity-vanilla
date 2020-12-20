using System;
using System.Threading.Tasks;
using MCLiveStatus.Domain.Models;
using MCLiveStatus.Domain.Services;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.ServerPingerSettingsStores
{
    public class ServerPingerSettingsStore : IPingConfigurableServerPingerSettingsStore
    {
        private readonly IServerPingerSettingsRepository _settingsRepository;

        private ServerPingerSettings _settings;
        private bool _isInvalidPingIntervalSeconds;
        private bool _hasDirtySettings;
        private bool _isLoading;

        private ServerPingerSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                HasDirtySettings = true;
                OnSettingsChanged();
            }
        }

        public bool AllowNotifyJoinable
        {
            get => _settings.AllowNotifyJoinable;
            set
            {
                _settings.AllowNotifyJoinable = value;
                HasDirtySettings = true;
                OnSettingsChanged();
            }
        }
        public bool AllowNotifyQueueJoinable
        {
            get => _settings.AllowNotifyQueueJoinable;
            set
            {
                _settings.AllowNotifyQueueJoinable = value;
                HasDirtySettings = true;
                OnSettingsChanged();
            }
        }
        public double PingIntervalSeconds
        {
            get => _settings.PingIntervalSeconds;
            set
            {
                _settings.PingIntervalSeconds = value;
                HasDirtySettings = true;
                OnSettingsChanged();
            }
        }

        public bool IsInvalidPingIntervalSeconds
        {
            get => _isInvalidPingIntervalSeconds;
            set
            {
                _isInvalidPingIntervalSeconds = value;
                OnValidationChanged();
            }
        }

        public bool HasDirtySettings
        {
            get => _hasDirtySettings;
            private set
            {
                _hasDirtySettings = value;
                OnHasDirtySettingsChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;
                OnIsLoadingChanged();
            }
        }

        public event Action SettingsChanged;
        public event Action HasDirtySettingsChanged;
        public event Action ValidationChanged;
        public event Action IsLoadingChanged;

        public ServerPingerSettingsStore(IServerPingerSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            _settings = new ServerPingerSettings() { PingIntervalSeconds = 5 };
        }

        /// <summary>
        /// Load the server pinger settings.
        /// </summary>
        /// <exception cref="Exception">Thrown if load fails.</exception>
        public async Task Load()
        {
            IsLoading = true;

            try
            {
                ServerPingerSettings storedSettings = await _settingsRepository.Load();

                if (storedSettings != null)
                {
                    Settings = storedSettings;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Save the server pinger settings.
        /// </summary>
        /// <exception cref="Exception">Thrown if save fails.</exception>
        public async Task Save()
        {
            Settings = await _settingsRepository.Save(Settings);
        }

        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke();
        }

        private void OnValidationChanged()
        {
            ValidationChanged?.Invoke();
        }

        private void OnIsLoadingChanged()
        {
            IsLoadingChanged?.Invoke();
        }

        private void OnHasDirtySettingsChanged()
        {
            HasDirtySettingsChanged?.Invoke();
        }
    }
}