namespace MCLiveStatus.PurityVanilla.Blazor.Stores.ServerPingerSettingsStores
{
    public interface IAutoRefreshServerPingerSettingsStore : IServerPingerSettingsStore
    {
        bool AutoRefreshEnabled { get; set; }
    }
}