using System;
using System.Net;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices;
using MCLiveStatus.ServerSettings.Domain.Models;
using Refit;

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

        public event Action SettingsChanged;
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
                if (_authenticationStore.IsLoggedIn)
                {
                    try
                    {
                        ServerPingerSettings settings = await _settingsService.GetSettings(_tokenStore.BearerAccessToken);
                        if (settings != null)
                        {
                            Settings = settings;
                        }
                    }
                    catch (ApiException ex)
                    {
                        if (ex.StatusCode == HttpStatusCode.NotFound)
                        {
                            Settings = CreateDefaultSettings();
                        }
                        else
                        {
                            throw;
                        }
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

        public async Task Save()
        {
            await _settingsService.SaveSettings(_tokenStore.BearerAccessToken, Settings);
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

            // TBD: Raise NeedsInitialization event?
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