using System.ComponentModel.DataAnnotations;

namespace MCLiveStatus.Authentication.Models.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}