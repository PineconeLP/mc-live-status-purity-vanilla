using System;
using System.Threading.Tasks;
using MCLiveStatus.Domain.Models;

namespace MCLiveStatus.Domain.Services
{
    public interface IServerPingerSettingsRepository
    {
        /// <summary>
        /// Load server pinger settings.
        /// </summary>
        /// <returns>The server pinger settings. Null if no settings exist.</returns>
        /// <exception cref="Exception">Thrown if getting settings fails.</exception>
        Task<ServerPingerSettings> Load();

        /// <summary>
        /// Save server pinger settings.
        /// </summary>
        /// <param name="settings">
        /// The settings to save. If settings have empty id, 
        /// the settings will be created rather than updated.
        /// </param>
        /// <returns>The server pinger settings with the saved id.</returns>
        /// <exception cref="Exception">Thrown if saving settings fails.</exception>
        Task<ServerPingerSettings> Save(ServerPingerSettings settings);
    }
}