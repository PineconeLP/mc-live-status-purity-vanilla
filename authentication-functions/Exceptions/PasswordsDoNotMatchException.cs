using System;

namespace MCLiveStatus.Authentication.Exceptions
{
    public class PasswordsDoNotMatchException : Exception
    {
        public PasswordsDoNotMatchException()
        {
        }

        public PasswordsDoNotMatchException(string message) : base(message)
        {
        }

        public PasswordsDoNotMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}