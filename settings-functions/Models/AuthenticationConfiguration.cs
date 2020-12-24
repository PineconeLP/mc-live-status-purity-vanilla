namespace MCLiveStatus.ServerSettings.Models
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}