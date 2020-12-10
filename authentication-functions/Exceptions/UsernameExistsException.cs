using System;
using System.Runtime.Serialization;

namespace MCLiveStatus.Authentication.Exceptions
{
    public class UsernameExistsException : Exception
    {
        public string Username { get; }

        public UsernameExistsException(string username)
        {
            Username = username;
        }

        public UsernameExistsException(string message, string username) : base(message)
        {
            Username = username;
        }

        public UsernameExistsException(string message, Exception innerException, string username) : base(message, innerException)
        {
            Username = username;
        }
    }
}