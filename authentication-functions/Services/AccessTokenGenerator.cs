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
    public class AccessTokenGenerator
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public AccessTokenGenerator(IOptions<TokenConfiguration> tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration.Value;
        }

        public string GenerateToken(User user, DateTime expiresAt)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.AccessTokenSecret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("name", user.Username)
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