using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components
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

        private bool IsLoading => SettingsStore.IsLoading;

        private bool CanSave { get; set; }

        protected override Task OnInitializedAsync()
        {
            SettingsStore.SettingsChanged += OnSettingsChanged;
            SettingsStore.ValidationChanged += OnSettingsChanged;
            SettingsStore.IsLoadingChanged += OnSettingsChanged;

            return base.OnInitializedAsync();
        }

        private void OnSettingsInput()
        {
            CanSave = true;
        }

        private void OnPingIntervalSecondsInput()
        {
            OnSettingsInput();
            SettingsStore.IsInvalidPingIntervalSeconds = false;
        }

        private async Task SaveSettings()
        {
            await SettingsStore.Save();
            CanSave = false;
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