using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using MCLiveStatus.ServerSettings.Services;
using Endpointer.Authentication.API.Services.Authenticators;
using MCLiveStatus.ServerSettings.Domain.Models;
using Endpointer.Authentication.API.Models;

namespace MCLiveStatus.ServerSettings.Functions
{
    public class GetSettingsFunction
    {
        private readonly IServerPingerSettingsRepository _settingsRepository;
        private readonly HttpRequestAuthenticator _authenticator;

        public GetSettingsFunction(IServerPingerSettingsRepository settingsRepository, HttpRequestAuthenticator authenticator)
        {
            _settingsRepository = settingsRepository;
            _authenticator = authenticator;
        }

        [FunctionName("GetSettingsFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pinger-settings")]
            HttpRequest request,
            ILogger log)
        {
            User user = await _authenticator.Authenticate(request);

            if (user == null)
            {
                return new UnauthorizedResult();
            }

            log.LogInformation("Getting server pinger settings for user id {userId}.", user.Id);

            ServerPingerSettings settings = await _settingsRepository.GetForUserId(user.Id);

            if (settings == null)
            {
                log.LogError("Could not find server pinger settings for user id {userId}.", user.Id);
                return new NotFoundResult();
            }

            log.LogInformation("Successfully retrieved server pinger settings for user id {userId}.", user.Id);

            return new OkObjectResult(settings);
        }
    }
}
