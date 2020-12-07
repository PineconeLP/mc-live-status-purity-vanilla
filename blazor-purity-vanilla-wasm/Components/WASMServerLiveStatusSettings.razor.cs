using System;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerPingerSettings;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Components
{
    public partial class WASMServerLiveStatusSettings : ComponentBase, IDisposable
    {
        [Inject]
        public ServerPingerSettingsStore ServerPingerSettingsStore { get; set; }

        private bool AutoRefreshEnabled
        {
            get => ServerPingerSettingsStore.AutoRefreshEnabled;
            set => ServerPingerSettingsStore.AutoRefreshEnabled = value;
        }

        protected override void OnInitialized()
        {
            ServerPingerSettingsStore.SettingsChanged += StateHasChanged;

            base.OnInitialized();
        }

        public void Dispose()
        {
            ServerPingerSettingsStore.SettingsChanged -= StateHasChanged;
        }
    }
}