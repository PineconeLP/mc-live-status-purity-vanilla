using System;
using System.Runtime.Serialization;

namespace MCLiveStatus.Authentication.Exceptions
{
    public class EmailExistsException : Exception
    {
        public string Email { get; }

        public EmailExistsException(string email)
        {
            Email = email;
        }

        public EmailExistsException(string message, string email) : base(message)
        {
            Email = email;
        }

        public EmailExistsException(string message, Exception innerException, string email) : base(message, innerException)
        {
            Email = email;
        }
    }
}