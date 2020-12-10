using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MCLiveStatus.Authentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MCLiveStatus.Authentication.Services
{
    public class RefreshTokenGenerator
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public RefreshTokenGenerator(IOptions<TokenConfiguration> tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration.Value;
        }

        public string GenerateToken(Guid userId, DateTime expiresAt)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.RefreshTokenSecret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", userId.ToString())
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _tokenConfiguration.Issuer,
                _tokenConfiguration.Audience,
                claims,
                DateTime.UtcNow,
                expiresAt,
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}