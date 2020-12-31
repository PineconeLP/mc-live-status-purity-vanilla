using System;
using System.Runtime.Serialization;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Exceptions
{
    public class ServerPingerSettingsNotFoundException : Exception
    {
        public ServerPingerSettingsNotFoundException()
        {
        }

        public ServerPingerSettingsNotFoundException(string message) : base(message)
        {
        }

        public ServerPingerSettingsNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}