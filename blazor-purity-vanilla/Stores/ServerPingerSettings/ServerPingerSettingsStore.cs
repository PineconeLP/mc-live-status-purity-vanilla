using System;
using MCLiveStatus.Domain.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores
{
    public class ServerPingerSettingsStore
    {
        private readonly ServerPingerSettings _settings;

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
        public double PingIntervalSeconds
        {
            get => _settings.PingIntervalSeconds;
            set
            {
                _settings.PingIntervalSeconds = value;
                OnSettingsChanged();
            }
        }

        private bool _isInvalidPingIntervalSeconds;
        public bool IsInvalidPingIntervalSeconds
        {
            get => _isInvalidPingIntervalSeconds;
            set
            {
                _isInvalidPingIntervalSeconds = value;
                OnValidationChanged();
            }
        }

        public event Action SettingsChanged;
        public event Action ValidationChanged;

        public ServerPingerSettingsStore()
        {
            _settings = new ServerPingerSettings()
            {
                PingIntervalSeconds = 5
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