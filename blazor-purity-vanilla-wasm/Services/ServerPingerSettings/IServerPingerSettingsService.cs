using System.Threading.Tasks;
using MCLiveStatus.ServerSettings.Domain.Models;
using Endpointer.Core.Client.Exceptions;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions;
using Refit;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices
{
    public interface IServerPingerSettingsService
    {
        /// <summary>
        /// Get server pinger settings for the current user.
        /// </summary>
        /// <returns>The user's settings.</returns>
        /// <exception cref="ServerPingerSettingsNotFoundException">Thrown if user authentication fails.</exception>
        /// <exception cref="UnauthorizedException">Thrown if user authentication fails.</exception>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task<ServerPingerSettings> GetSettings();

        /// <summary>
        /// Save server pinger settings for the current user.
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        /// <exception cref="UnauthorizedException">Thrown if user authentication fails.</exception>
        /// <exception cref="Exception">Thrown if request fails.</exception>
        Task SaveSettings(ServerPingerSettings settings);
    }
}