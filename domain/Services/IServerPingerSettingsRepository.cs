using System.Threading.Tasks;
using MCLiveStatus.Domain.Models;

namespace MCLiveStatus.Domain.Services
{
    public interface IServerPingerSettingsRepository
    {
        Task<ServerPingerSettings> Load();
        Task Save(ServerPingerSettings settings);
    }
}