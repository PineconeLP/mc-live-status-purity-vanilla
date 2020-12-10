using System;

namespace MCLiveStatus.Authentication.Models.Responses
{
    public class AuthenticatedUserResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpireTime { get; set; }
    }
}