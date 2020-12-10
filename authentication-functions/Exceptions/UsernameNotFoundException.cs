using System;

namespace MCLiveStatus.Authentication.Exceptions
{
    public class UsernameNotFoundException : Exception
    {
        public string Username { get; }

        public UsernameNotFoundException(string username)
        {
            Username = username;
        }

        public UsernameNotFoundException(string message, string username) : base(message)
        {
            Username = username;
        }

        public UsernameNotFoundException(string message, Exception innerException, string username) : base(message, innerException)
        {
            Username = username;
        }
    }
}