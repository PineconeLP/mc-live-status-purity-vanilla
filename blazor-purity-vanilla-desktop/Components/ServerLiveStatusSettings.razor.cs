using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.ServerPingerSettingsStores;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Components
{
    public partial class ServerLiveStatusSettings : ComponentBase, IDisposable
    {
        [Parameter]
        public bool HasQueue { get; set; }

        [Parameter]
        public int MaxPlayersExcludingQueue { get; set; }

        [Parameter]
        public int MaxPlayers { get; set; }

        [Inject]
        public ServerPingerSettingsStore SettingsStore { get; set; }

        private bool AllowNotifyJoinable
        {
            get => SettingsStore.AllowNotifyJoinable;
            set => SettingsStore.AllowNotifyJoinable = value;
        }

        private bool AllowNotifyQueueJoinable
        {
            get => SettingsStore.AllowNotifyQueueJoinable;
            set => SettingsStore.AllowNotifyQueueJoinable = value;
        }

        private double PingIntervalSeconds
        {
            get => SettingsStore.PingIntervalSeconds;
            set => SettingsStore.PingIntervalSeconds = value;
        }

        private bool IsInvalidPingIntervalSeconds => SettingsStore.IsInvalidPingIntervalSeconds;
        private string InvalidPingIntervalSecondsClass => IsInvalidPingIntervalSeconds ? "is-invalid" : "";
        private bool HasDirtySettings { get; set; }

        private bool IsLoading => SettingsStore.IsLoading;
        private bool CanSave => HasDirtySettings && !IsInvalidPingIntervalSeconds;

        protected override Task OnInitializedAsync()
        {
            SettingsStore.SettingsChanged += OnSettingsChanged;
            SettingsStore.ValidationChanged += OnSettingsChanged;
            SettingsStore.IsLoadingChanged += OnSettingsChanged;

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

        private void OnSettingsChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            SettingsStore.SettingsChanged -= OnSettingsChanged;
            SettingsStore.ValidationChanged -= OnSettingsChanged;
        }
    }
}