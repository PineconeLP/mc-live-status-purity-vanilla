using System;
using System.Linq;
using System.Threading.Tasks;
using MCLiveStatus.Authentication.Models;
using Microsoft.AspNetCore.Http;

namespace MCLiveStatus.Authentication.Services
{
    public class HttpRequestAuthenticator
    {
        private const string BEARER_PREFIX = "Bearer ";

        private readonly AccessTokenDecoder _tokenDecoder;

        public HttpRequestAuthenticator(AccessTokenDecoder tokenDecoder)
        {
            _tokenDecoder = tokenDecoder;
        }

        public async Task<User> Authenticate(HttpRequest request)
        {
            User user = null;

            // Ensure authorization header has a token.
            string rawBearerToken = request.Headers["Authorization"].FirstOrDefault();
            if (rawBearerToken != null && rawBearerToken.StartsWith(BEARER_PREFIX, StringComparison.InvariantCultureIgnoreCase))
            {
                // Validate the token and get the user's email.
                string token = rawBearerToken.Substring(BEARER_PREFIX.Length);
                try
                {
                    user = await _tokenDecoder.GetUserFromToken(token);
                }
                catch (Exception)
                {
                    user = null;
                }
            }

            return user;
        }
    }
}