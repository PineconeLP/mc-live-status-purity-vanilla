using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Models;
using MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.ServerPingers
{
    public interface IServerPinger
    {
        /// <summary>
        /// Ping the server.
        /// </summary>
        /// <returns>The pinged server's status.</returns>
        /// <exception cref="ServerPingFailedException">Thrown if pinging the server fails.</exception>
        /// <exception cref="Exception">Thrown if unknown ping failure.</exception>
        Task<ServerPingResponse> Ping();
    }
}