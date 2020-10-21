using System;
using MCLiveStatus.PurityVanilla.Blazor.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingerSettingsStores
{
    public class ServerStatusPingerSettingsStore
    {
        private readonly ServerStatusPingerSettings _settings;

        public bool AllowNotifyJoinable
        {
            get => _settings.AllowNotifyJoinable;
            set
            {
                _settings.AllowNotifyJoinable = value;
                OnSettingsChanged();
            }
        }
        public bool AllowNotifyQueueJoinable
        {
            get => _settings.AllowNotifyQueueJoinable;
            set
            {
                _settings.AllowNotifyQueueJoinable = value;
                OnSettingsChanged();
            }
        }
        public double ServerStatusPingIntervalSeconds
        {
            get => _settings.ServerStatusPingIntervalSeconds;
            set
            {
                _settings.ServerStatusPingIntervalSeconds = value;
                OnSettingsChanged();
            }
        }

        private bool _isInvalidServerStatusPingIntervalSeconds;
        public bool IsInvalidServerStatusPingIntervalSeconds
        {
            get => _isInvalidServerStatusPingIntervalSeconds;
            set
            {
                _isInvalidServerStatusPingIntervalSeconds = value;
                OnValidationChanged();
            }
        }

        public event Action SettingsChanged;
        public event Action ValidationChanged;

        public ServerStatusPingerSettingsStore()
        {
            _settings = new ServerStatusPingerSettings()
            {
                ServerStatusPingIntervalSeconds = 5
            };
        }

        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke();
        }

        private void OnValidationChanged()
        {
            ValidationChanged?.Invoke();
        }
    }
}