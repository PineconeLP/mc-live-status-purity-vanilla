using System;
using System.IO;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.NotificationSupportCheckers;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Components
{
    public partial class WASMServerLiveStatusSettings : ComponentBase, IDisposable
    {
        [Inject]
        public IAutoRefreshServerPingerSettingsStore ServerPingerSettingsStore { get; set; }

        [Inject]
        public IServerStatusPingerStore ServerStatusPingerStore { get; set; }

        [Inject]
        public AuthenticationStore AuthenticationStore { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public NotificationSupportChecker NotificationSupportChecker { get; set; }

        private bool AutoRefreshEnabled
        {
            get => ServerPingerSettingsStore.AutoRefreshEnabled;
            set => ServerPingerSettingsStore.AutoRefreshEnabled = value;
        }

        private bool AllowNotifyJoinable
        {
            get => ServerPingerSettingsStore.AllowNotifyJoinable;
            set => ServerPingerSettingsStore.AllowNotifyJoinable = value;
        }

        private bool AllowNotifyQueueJoinable
        {
            get => ServerPingerSettingsStore.AllowNotifyQueueJoinable;
            set => ServerPingerSettingsStore.AllowNotifyQueueJoinable = value;
        }

        private bool HasQueue => ServerStatusPingerStore.ServerDetails.HasQueue;
        private int MaxPlayers => ServerStatusPingerStore.ServerDetails.MaxPlayers;
        private int MaxPlayersExcludingQueue => ServerStatusPingerStore.ServerDetails.MaxPlayersExcludingQueue;

        private bool IsLoggedIn => AuthenticationStore.IsLoggedIn;
        private bool CanSave => !IsLoading && !IsSaving && AuthenticationStore.IsLoggedIn && ServerPingerSettingsStore.HasDirtySettings;

        private bool IsLoading { get; set; }
        private bool IsSaving { get; set; }
        private bool IsNotificationSupported { get; set; }

        private string LoginRedirect => Path.Combine(NavigationManager.BaseUri, "login");

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            StateHasChanged();

            ServerPingerSettingsStore.SettingsChanged += StateHasChanged;
            ServerPingerSettingsStore.HasDirtySettingsChanged += StateHasChanged;
            ServerStatusPingerStore.StateChanged += StateHasChanged;
            AuthenticationStore.IsLoggedInChanged += RefreshServerPingerSettings;

            await ServerPingerSettingsStore.Load();
            IsNotificationSupported = await NotificationSupportChecker.IsNotificationSupported();

            IsLoading = false;
            StateHasChanged();

            await base.OnInitializedAsync();
        }

        private async Task SaveSettings()
        {
            IsSaving = true;
            StateHasChanged();

            await ServerPingerSettingsStore.Save();

            IsSaving = false;
            StateHasChanged();
        }

        private async void RefreshServerPingerSettings()
        {
            IsLoading = true;
            StateHasChanged();

            if (AuthenticationStore.IsLoggedIn)
            {
                await ServerPingerSettingsStore.Load();
            }

            IsLoading = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            ServerPingerSettingsStore.SettingsChanged -= StateHasChanged;
            ServerPingerSettingsStore.HasDirtySettingsChanged -= StateHasChanged;
            ServerStatusPingerStore.StateChanged -= StateHasChanged;
            AuthenticationStore.IsLoggedInChanged -= RefreshServerPingerSettings;
        }
    }
}