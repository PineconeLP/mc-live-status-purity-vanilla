using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MCLiveStatus.ServerSettings.Services;
using MCLiveStatus.ServerSettings.Domain.Models;
using Endpointer.Core.API.Models;
using Endpointer.Core.API.Http;

namespace MCLiveStatus.ServerSettings.Functions
{
    public class PostSettingsFunction
    {
        private readonly IServerPingerSettingsRepository _settingsRepository;
        private readonly HttpRequestAuthenticator _authenticator;

        public PostSettingsFunction(IServerPingerSettingsRepository settingsRepository, HttpRequestAuthenticator authenticator)
        {
            _settingsRepository = settingsRepository;
            _authenticator = authenticator;
        }

        [FunctionName("PostSettingsFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pinger-settings")]
            ServerPingerSettings settings,
            HttpRequest request,
            ILogger log)
        {
            User user = await _authenticator.Authenticate(request);

            if (user == null)
            {
                log.LogError("Unable to authenticate user.");
                return new UnauthorizedResult();
            }

            log.LogInformation("Saving server pinger settings for user id {userId}.", user.Id);
            log.LogInformation("Auto refresh enabled: {autoRefreshEnabled}", settings.AutoRefresh);
            log.LogInformation("Allow notify joinable: {allowNotifyJoinable}", settings.AllowNotifyJoinable);
            log.LogInformation("Allow notify queue joinable: {allowNotifyQueueJoinable}", settings.AllowNotifyQueueJoinable);

            await _settingsRepository.SaveForUserId(user.Id, settings);

            log.LogInformation("Successfully saved server pinger settings.");

            return new OkResult();
        }
    }
}
