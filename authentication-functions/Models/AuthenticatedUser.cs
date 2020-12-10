using System;

namespace MCLiveStatus.Authentication.Models
{
    public class AuthenticatedUser
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpireTime { get; set; }
        public string RefreshToken { get; set; }
    }
}