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

        private bool IsLoggedIn => ServerPingerSettingsStore.HasSettingsAuthentication;
        private bool CanSave => !IsLoading && !IsSaving && IsLoggedIn && ServerPingerSettingsStore.HasDirtySettings;

        private bool IsLoading { get; set; }
        private bool LoadFailed { get; set; }
        private bool IsSaving { get; set; }
        private bool SaveFailed { get; set; }
        private bool IsNotificationSupported { get; set; }

        private string LoginRedirect => Path.Combine(NavigationManager.BaseUri, "login");

        protected override async Task OnInitializedAsync()
        {
            ServerPingerSettingsStore.SettingsChanged += StateHasChanged;
            ServerPingerSettingsStore.HasSettingsAuthenticationChanged += HandleHasSettingsAuthenticationChanged;
            ServerPingerSettingsStore.HasDirtySettingsChanged += StateHasChanged;
            ServerStatusPingerStore.StateChanged += StateHasChanged;

            await Load();

            await base.OnInitializedAsync();
        }

        private async Task Load()
        {
            IsLoading = true;
            LoadFailed = false;
            StateHasChanged();

            try
            {
                IsNotificationSupported = await NotificationSupportChecker.IsNotificationSupported();
                await ServerPingerSettingsStore.Load();
            }
            catch (Exception)
            {
                LoadFailed = true;
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async Task SaveSettings()
        {
            IsSaving = true;
            SaveFailed = false;
            StateHasChanged();

            try
            {
                await ServerPingerSettingsStore.Save();
            }
            catch (Exception)
            {
                SaveFailed = true;
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        private async void HandleHasSettingsAuthenticationChanged()
        {
            if (IsLoggedIn)
            {
                await Load();
            }

            StateHasChanged();
        }

        public void Dispose()
        {
            ServerPingerSettingsStore.SettingsChanged -= StateHasChanged;
            ServerPingerSettingsStore.HasSettingsAuthenticationChanged -= HandleHasSettingsAuthenticationChanged;
            ServerPingerSettingsStore.HasDirtySettingsChanged -= StateHasChanged;
            ServerStatusPingerStore.StateChanged -= StateHasChanged;
        }
    }
}