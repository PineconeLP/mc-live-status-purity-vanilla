using System;
using System.Runtime.Serialization;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions
{
    public class ServerPingFailedException : Exception
    {
        public ServerPingFailedException()
        {
        }

        public ServerPingFailedException(string message) : base(message)
        {
        }

        public ServerPingFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}