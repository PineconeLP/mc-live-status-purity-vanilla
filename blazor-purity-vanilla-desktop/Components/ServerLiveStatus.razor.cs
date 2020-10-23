using System;
using System.Globalization;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Desktop.Stores.ServerStatusPingers;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Components
{
    public partial class ServerLiveStatus : ComponentBase, IDisposable
    {
        [Inject]
        public ServerStatusPingerStore ServerStatusPingerStore { get; set; }

        private string Host => ServerStatusPingerStore.ServerDetails.Host;
        private int Port => ServerStatusPingerStore.ServerDetails.Port;
        private string Name => ServerStatusPingerStore.ServerDetails.Name;
        private bool HasQueue => ServerStatusPingerStore.ServerDetails.HasQueue;
        private int OnlinePlayers => ServerStatusPingerStore.ServerDetails.OnlinePlayers;
        private int MaxPlayers => ServerStatusPingerStore.ServerDetails.MaxPlayers;
        private int MaxPlayersExcludingQueue => ServerStatusPingerStore.ServerDetails.MaxPlayersExcludingQueue;
        private bool IsFull => ServerStatusPingerStore.ServerDetails.IsFull;
        private bool IsFullExcludingQueue => ServerStatusPingerStore.ServerDetails.IsFullExcludingQueue;
        private bool IsLoading => !ServerStatusPingerStore.ServerDetails.HasData;
        private string LastUpdateTimeDisplay => ToDisplayString(ServerStatusPingerStore.LastUpdateTime);
        private bool HasUpdateError => ServerStatusPingerStore.HasUpdateError;
        private string LastUpdateErrorTimeDisplay => ToDisplayString(ServerStatusPingerStore.LastUpdateErrorTime);

        private bool _canRefresh;
        private bool CanRefresh
        {
            get => _canRefresh;
            set
            {
                _canRefresh = value;
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ServerStatusPingerStore.StateChanged += OnPingerStoreStateChanged;

                await InitializePingerStore();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task InitializePingerStore()
        {
            await ServerStatusPingerStore.Load();
            CanRefresh = true;
        }

        private async Task Refresh()
        {
            CanRefresh = false;
            await ServerStatusPingerStore.RefreshServerStatus();
            CanRefresh = true;
        }

        private string ToDisplayString(DateTime dateTime)
        {
            return dateTime.ToString("MMM d 'at' hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        private void OnPingerStoreStateChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            ServerStatusPingerStore.StateChanged -= OnPingerStoreStateChanged;
        }
    }
}