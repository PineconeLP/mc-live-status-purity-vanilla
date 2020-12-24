using System;
using System.Threading.Tasks;
using MCLiveStatus.ServerSettings.Domain.Models;

namespace MCLiveStatus.ServerSettings.Services
{
    public interface IServerPingerSettingsRepository
    {
        Task<ServerPingerSettings> GetForUserId(Guid userId);

        Task SaveForUserId(Guid userId, ServerPingerSettings settings);
    }
}