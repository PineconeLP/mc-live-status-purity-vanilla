using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using MCLiveStatus.ServerSettings.Domain.Models;

namespace MCLiveStatus.ServerSettings.Services
{
    public class FirebaseServerPingerSettingsRepository : IServerPingerSettingsRepository
    {
        private readonly FirebaseClient _client;

        public FirebaseServerPingerSettingsRepository(FirebaseClient client)
        {
            _client = client;
        }

        public async Task<ServerPingerSettings> GetForUserId(Guid userId)
        {
            return await _client
                .Child("server-pinger-settings")
                .Child(userId.ToString)
                .OnceSingleAsync<ServerPingerSettings>();
        }

        public async Task SaveForUserId(Guid userId, ServerPingerSettings settings)
        {
            await _client
                .Child("server-pinger-settings")
                .Child(userId.ToString)
                .PutAsync(settings);
        }
    }
}