using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingerSettingsStores;
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
        public ServerStatusPingerSettingsStore SettingsStore { get; set; }

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

        private double ServerStatusPingIntervalSeconds
        {
            get => SettingsStore.ServerStatusPingIntervalSeconds;
            set => SettingsStore.ServerStatusPingIntervalSeconds = value;
        }

        private bool IsInvalidServerStatusPingIntervalSeconds => SettingsStore.IsInvalidServerStatusPingIntervalSeconds;
        private string InvalidServerStatusPingIntervalSecondsClass => IsInvalidServerStatusPingIntervalSeconds ? "is-invalid" : "";

        protected override Task OnInitializedAsync()
        {
            SettingsStore.SettingsChanged += OnSettingsChanged;
            SettingsStore.ValidationChanged += OnSettingsChanged;

            return base.OnInitializedAsync();
        }

        private void ClearIsInvalidServerStatusPingIntervalSeconds()
        {
            SettingsStore.IsInvalidServerStatusPingIntervalSeconds = false;
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