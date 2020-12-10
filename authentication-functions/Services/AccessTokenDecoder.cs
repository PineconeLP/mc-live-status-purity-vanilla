using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MCLiveStatus.Authentication.Models;
using Microsoft.IdentityModel.Tokens;

namespace MCLiveStatus.Authentication.Services
{
    public class AccessTokenDecoder
    {
        private readonly TokenValidationParameters _validationParameters;

        public AccessTokenDecoder(TokenValidationParameters validationParameters)
        {
            _validationParameters = validationParameters;
        }

        public Task<User> GetUserFromToken(string rawToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal token = handler.ValidateToken(rawToken, _validationParameters, out SecurityToken validatedToken);

            string rawId = token.FindFirstValue("id");
            if (!Guid.TryParse(rawId, out Guid id))
            {
                throw new Exception("Unable to parse ID from JWT.");
            }

            string email = token.FindFirstValue("email");
            string name = token.FindFirstValue("name");

            return Task.FromResult(new User()
            {
                Id = id,
                Username = name
            });
        }
    }
}