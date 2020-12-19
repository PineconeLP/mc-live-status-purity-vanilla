using System;
using System.Threading.Tasks;
using MCLiveStatus.ServerSettings.Domain.Models;

namespace MCLiveStatus.ServerSettings.Services
{
    public class FirebaseServerPingerSettingsRepository : IServerPingerSettingsRepository
    {
        public async Task<ServerPingerSettings> GetForUserId(Guid userId)
        {
            return new ServerPingerSettings();
        }

        public async Task SaveForUserId(Guid userId, ServerPingerSettings settings)
        {

        }
    }
}