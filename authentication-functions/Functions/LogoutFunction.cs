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

namespace MCLiveStatus.Authentication.Functions
{
    public class LogoutFunction
    {
        private readonly Authenticator _authenticator;
        private readonly HttpRequestAuthenticator _requestAuthenticator;

        public LogoutFunction(Authenticator authenticator, HttpRequestAuthenticator requestAuthenticator)
        {
            _authenticator = authenticator;
            _requestAuthenticator = requestAuthenticator;
        }

        [FunctionName("LogoutFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "logout")]
            HttpRequest request,
            ILogger log)
        {

            User user = await _requestAuthenticator.Authenticate(request);
            if (user == null)
            {
                return new UnauthorizedResult();
            }

            await _authenticator.Logout(user.Id);

            return new NoContentResult();
        }
    }
}
