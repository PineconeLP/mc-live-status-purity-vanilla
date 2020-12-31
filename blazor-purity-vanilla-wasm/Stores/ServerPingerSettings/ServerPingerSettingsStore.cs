using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices;
using MCLiveStatus.ServerSettings.Domain.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettingStores
{
    public class ServerPingerSettingsStore : IAutoRefreshServerPingerSettingsStore
    {
        private readonly ITokenStore _tokenStore;
        private readonly AuthenticationStore _authenticationStore;
        private readonly IServerPingerSettingsService _settingsService;

        private TaskCompletionSource<object> _initializeTask;
        private bool _hasDirtySettings;
        private bool _isLoading;

        private ServerPingerSettings _settings;
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

        public bool AutoRefreshEnabled
        {
            get => Settings.AutoRefresh;
            set
            {
                Settings.AutoRefresh = value;
                HasDirtySettings = true;
                OnSettingsChanged();
            }
        }

        public bool AllowNotifyJoinable
        {
            get => Settings.AllowNotifyJoinable;
            set
            {
                Settings.AllowNotifyJoinable = value;
                HasDirtySettings = true;
                OnSettingsChanged();
            }
        }

        public bool AllowNotifyQueueJoinable
        {
            get => Settings.AllowNotifyQueueJoinable;
            set
            {
                Settings.AllowNotifyQueueJoinable = value;
                HasDirtySettings = true;
                OnSettingsChanged();
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

        public bool HasSettingsAuthentication => _authenticationStore.IsLoggedIn;

        public event Action SettingsChanged;
        public event Action HasSettingsAuthenticationChanged;
        public event Action HasDirtySettingsChanged;
        public event Action IsLoadingChanged;

        public ServerPingerSettingsStore(ITokenStore tokenStore, AuthenticationStore authenticationStore, IServerPingerSettingsService settingsService)
        {
            _tokenStore = tokenStore;
            _authenticationStore = authenticationStore;
            _settingsService = settingsService;

            Settings = CreateDefaultSettings();

            _authenticationStore.IsLoggedInChanged += OnIsLoggedInChanged;
        }

        /// <inheritdoc/>
        public async Task Load()
        {
            await _authenticationStore.Initialize();

            if (_initializeTask == null)
            {
                _initializeTask = new TaskCompletionSource<object>();

                await LoadSettings();

                _initializeTask.TrySetResult(null);
            }

            await _initializeTask.Task;
        }

        private async Task LoadSettings()
        {
            IsLoading = true;

            try
            {
                if (HasSettingsAuthentication)
                {
                    try
                    {
                        Settings = await _settingsService.GetSettings();
                    }
                    catch (ServerPingerSettingsNotFoundException)
                    {
                        Settings = CreateDefaultSettings();
                    }
                    catch (Exception)
                    {
                        Settings = CreateDefaultSettings();
                        throw;
                    }
                }
                else
                {
                    Settings = CreateDefaultSettings();
                }
            }
            finally
            {
                HasDirtySettings = false;
                IsLoading = false;
            }
        }

        /// <inheritdoc/>
        public async Task Save()
        {
            await _settingsService.SaveSettings(Settings);
            HasDirtySettings = false;
        }

        private ServerPingerSettings CreateDefaultSettings()
        {
            return new ServerPingerSettings()
            {
                AutoRefresh = true
            };
        }

        private void OnIsLoggedInChanged()
        {
            _initializeTask?.TrySetResult(null);
            _initializeTask = null;

            HasSettingsAuthenticationChanged?.Invoke();
        }

        private void OnIsLoadingChanged()
        {
            IsLoadingChanged?.Invoke();
        }

        private void OnSettingsChanged()
        {
            SettingsChanged?.Invoke();
        }

        private void OnHasDirtySettingsChanged()
        {
            HasDirtySettingsChanged?.Invoke();
        }
    }
}