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
    public class LoginFunction
    {
        private readonly Authenticator _authenticator;

        public LoginFunction(Authenticator authenticator)
        {
            _authenticator = authenticator;
        }

        [FunctionName("LoginFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")]
            LoginRequest loginRequest,
            HttpRequest request,
            ILogger log)
        {
            try
            {
                AuthenticatedUser authenticatedUser = await _authenticator.Login(
                    loginRequest.Username,
                    loginRequest.Password);

                AuthenticatedUserResponse authenticatedUserResponse = new AuthenticatedUserResponse()
                {
                    AccessToken = authenticatedUser.AccessToken,
                    RefreshToken = authenticatedUser.RefreshToken,
                    AccessTokenExpireTime = authenticatedUser.AccessTokenExpireTime
                };

                return new OkObjectResult(authenticatedUserResponse);
            }
            catch (UsernameNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (InvalidPasswordException)
            {
                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.StackTrace);
                return new StatusCodeResult(500);
            }
        }
    }
}
