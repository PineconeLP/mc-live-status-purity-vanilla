using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MCLiveStatus.Authentication.Exceptions;
using MCLiveStatus.Authentication.Models.Requests;
using MCLiveStatus.Authentication.Services;
using MCLiveStatus.Authentication.Models;
using MCLiveStatus.Authentication.Models.Responses;

namespace MCLiveStatus.Authentication.Functions
{
    public class RefreshFunction
    {
        private readonly Authenticator _authenticator;

        public RefreshFunction(Authenticator authenticator)
        {
            _authenticator = authenticator;
        }

        [FunctionName("RefreshFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "refresh")]
            RefreshTokenRequest refreshTokenRequest,
            HttpRequest request,
            ILogger log)
        {
            try
            {
                AuthenticatedUser authenticatedUser = await _authenticator.Refresh(
                    refreshTokenRequest.RefreshToken
                );

                AuthenticatedUserResponse authenticatedUserResponse = new AuthenticatedUserResponse()
                {
                    AccessToken = authenticatedUser.AccessToken,
                    RefreshToken = authenticatedUser.RefreshToken,
                    AccessTokenExpireTime = authenticatedUser.AccessTokenExpireTime
                };

                return new OkObjectResult(authenticatedUserResponse);
            }
            catch (InvalidRefreshTokenException)
            {
                return new BadRequestResult();
            }
        }
    }
}
