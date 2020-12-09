using System;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettings;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Components
{
    public partial class WASMServerLiveStatusSettings : ComponentBase, IDisposable
    {
        [Inject]
        public ServerPingerSettingsStore ServerPingerSettingsStore { get; set; }

        [Inject]
        public IServerStatusPingerStore ServerStatusPingerStore { get; set; }

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

        protected override void OnInitialized()
        {
            ServerPingerSettingsStore.SettingsChanged += StateHasChanged;
            ServerStatusPingerStore.StateChanged += StateHasChanged;

            base.OnInitialized();
        }

        public void Dispose()
        {
            ServerPingerSettingsStore.SettingsChanged -= StateHasChanged;
            ServerStatusPingerStore.StateChanged -= StateHasChanged;
        }
    }
}