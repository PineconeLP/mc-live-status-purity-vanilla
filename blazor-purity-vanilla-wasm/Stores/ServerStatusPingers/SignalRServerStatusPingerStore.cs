using System;
using System.Threading.Tasks;
using Append.Blazor.Notifications;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.ServerStatusNotifiers;
using MCLiveStatus.PurityVanilla.Blazor.Stores.ServerStatusPingers;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers;
using Microsoft.AspNetCore.SignalR.Client;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Stores.ServerStatusPingers
{
    public class SignalRServerStatusPingerStore : IServerStatusPingerStore
    {
        private readonly IServerPinger _pinger;
        private readonly IServerStatusNotifier _serverStatusNotifier;
        private readonly string _negotiateUrl;
        private readonly PingedServerDetails _serverDetails;

        private HubConnection _connection;

        public IPingedServerDetails ServerDetails => _serverDetails;

        public DateTime LastUpdateTime { get; private set; }

        public bool HasUpdateError { get; private set; }

        public DateTime LastUpdateErrorTime { get; private set; }

        public event Action StateChanged;

        public SignalRServerStatusPingerStore(IServerPinger pinger,
            IServerStatusNotifier serverStatusNotifier,
            string negotiateUrl)
        {
            _pinger = pinger;
            _serverStatusNotifier = serverStatusNotifier;
            _negotiateUrl = negotiateUrl;

            _serverDetails = new PingedServerDetails(new ServerDetails()
            {
                Name = "Purity Vanilla",
                Host = "purityvanilla.com",
                Port = 25565,
                HasQueue = true,
                MaxPlayersExcludingQueue = 75
            });
        }

        public async Task Load()
        {
            await LoadServerStatus();

            await _serverStatusNotifier.RequestPermission();

            _connection = new HubConnectionBuilder()
                .WithUrl(_negotiateUrl)
                .Build();

            _connection.On<int, int>("ping", OnNotificationPingCompleted);

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                OnPingFailed(ex);
            }
        }

        private async Task LoadServerStatus()
        {
            try
            {
                ServerPingResponse response = await _pinger.Ping();
                OnPingCompleted(response.OnlinePlayers, response.MaxPlayers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                OnPingFailed(ex);
            }
        }

        public async Task RefreshServerStatus()
        {
            try
            {
                ServerPingResponse response = await _pinger.Ping();
                OnNotificationPingCompleted(response.OnlinePlayers, response.MaxPlayers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                OnPingFailed(ex);
            }
        }

        // TODO: Pull this duplication out between this and the ServerStatusPingerStore.

        private void OnNotificationPingCompleted(int online, int max)
        {
            bool wasFull = _serverDetails.IsFull;
            bool wasFullExcludingQueue = _serverDetails.IsFullExcludingQueue;

            OnPingCompleted(online, max);

            TryNotify(wasFull, wasFullExcludingQueue);
        }

        private void OnPingCompleted(int online, int max)
        {
            HasUpdateError = false;
            LastUpdateTime = DateTime.Now;

            _serverDetails.HasData = true;
            _serverDetails.OnlinePlayers = online;
            _serverDetails.MaxPlayers = max;

            OnStateChanged();
        }

        private void OnPingFailed(Exception ex)
        {
            HasUpdateError = true;
            LastUpdateErrorTime = DateTime.Now;
            OnStateChanged();
        }

        private void TryNotify(bool wasFull, bool wasFullExcludingQueue)
        {
            if (ServerDetails.HasQueue)
            {
                if (wasFullExcludingQueue && !ServerDetails.IsFullExcludingQueue)
                {
                    _serverStatusNotifier.NotifyJoinableExludingQueue(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayersExcludingQueue);
                }
                else if (wasFull && !ServerDetails.IsFull)
                {
                    _serverStatusNotifier.NotifyQueueJoinable(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayers);
                }
            }
            else if (wasFull && !ServerDetails.IsFull)
            {
                _serverStatusNotifier.NotifyJoinable(ServerDetails.Name, ServerDetails.OnlinePlayers, ServerDetails.MaxPlayers);
            }
        }

        public void Dispose()
        {
            _connection?.DisposeAsync();
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}