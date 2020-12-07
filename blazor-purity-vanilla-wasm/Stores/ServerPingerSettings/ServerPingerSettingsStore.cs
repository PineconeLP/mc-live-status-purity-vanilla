using System;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettings
{
    public class ServerPingerSettingsStore
    {
        private bool _autoRefreshEnabled;

        public event Action SettingsChanged;

        public bool AutoRefreshEnabled
        {
            get => _autoRefreshEnabled;
            set
            {
                _autoRefreshEnabled = value;
                OnSettingsChanged();
            }
        }

        public ServerPingerSettingsStore()
        {
            AutoRefreshEnabled = true;
        }

        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke();
        }
    }
}