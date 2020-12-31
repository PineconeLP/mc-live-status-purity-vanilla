using System.Threading.Tasks;
using MCLiveStatus.ServerSettings.Domain.Models;
using Endpointer.Core.Client.Exceptions;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions;
using Refit;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingerSettingsServices
{
    public interface IAPIServerPingerSettingsService
    {
        /// <summary>
        /// Save server pinger settings for the current user.
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        /// <exception cref="ApiException">Thrown if request fails.</exception>
        [Get("")]
        Task<ServerPingerSettings> GetSettings();

        /// <summary>
        /// Save server pinger settings for the current user.
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        /// <exception cref="ApiException">Thrown if request fails.</exception>
        [Post("")]
        Task SaveSettings([Body] ServerPingerSettings settings);
    }
}