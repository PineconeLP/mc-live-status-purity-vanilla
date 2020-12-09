using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components
{
    public partial class ServerLiveStatusSettings : ComponentBase, IDisposable
    {
        [Inject]
        public IServerStatusPingerStore ServerStatusPingerStore { get; set; }

        [Inject]
        public IPingConfigurableServerPingerSettingsStore SettingsStore { get; set; }

        private bool AllowNotifyJoinable
        {
            get => SettingsStore.AllowNotifyJoinable;
            set
            {
                SettingsStore.AllowNotifyJoinable = value;
                OnSettingsInput();
            }
        }

        private bool AllowNotifyQueueJoinable
        {
            get => SettingsStore.AllowNotifyQueueJoinable;
            set
            {
                SettingsStore.AllowNotifyQueueJoinable = value;
                OnSettingsInput();
            }
        }

        private double PingIntervalSeconds
        {
            get => SettingsStore.PingIntervalSeconds;
            set => SettingsStore.PingIntervalSeconds = value;
        }

        private bool HasQueue => ServerStatusPingerStore.ServerDetails.HasQueue;
        private int MaxPlayers => ServerStatusPingerStore.ServerDetails.MaxPlayers;
        private int MaxPlayersExcludingQueue => ServerStatusPingerStore.ServerDetails.MaxPlayersExcludingQueue;

        private bool IsInvalidPingIntervalSeconds => SettingsStore.IsInvalidPingIntervalSeconds;
        private string InvalidPingIntervalSecondsClass => IsInvalidPingIntervalSeconds ? "is-invalid" : "";
        private bool HasDirtySettings { get; set; }

        private bool IsLoading => SettingsStore.IsLoading;
        private bool CanSave => HasDirtySettings && !IsInvalidPingIntervalSeconds;

        protected override Task OnInitializedAsync()
        {
            SettingsStore.SettingsChanged += OnStateChanged;
            SettingsStore.ValidationChanged += OnStateChanged;
            SettingsStore.IsLoadingChanged += OnStateChanged;

            ServerStatusPingerStore.StateChanged += OnStateChanged;

            return base.OnInitializedAsync();
        }

        private void OnSettingsInput()
        {
            HasDirtySettings = true;
        }

        private void OnPingIntervalSecondsInput()
        {
            OnSettingsInput();
            SettingsStore.IsInvalidPingIntervalSeconds = false;
        }

        private async Task SaveSettings()
        {
            await SettingsStore.Save();
            HasDirtySettings = false;
        }

        private void OnStateChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            SettingsStore.SettingsChanged -= OnStateChanged;
            SettingsStore.ValidationChanged -= OnStateChanged;
            SettingsStore.IsLoadingChanged -= OnStateChanged;

            ServerStatusPingerStore.StateChanged -= OnStateChanged;
        }
    }
}