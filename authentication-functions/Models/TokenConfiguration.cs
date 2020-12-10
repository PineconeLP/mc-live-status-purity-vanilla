namespace MCLiveStatus.Authentication.Models
{
    public class TokenConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public string RefreshTokenSecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}