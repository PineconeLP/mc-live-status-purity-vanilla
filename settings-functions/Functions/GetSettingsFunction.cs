using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace MCLiveStatus.ServerSettings.Functions
{
    public class GetSettingsFunction
    {
        [FunctionName("GetSettingsFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{userId:Guid}")]
            HttpRequest request,
            Guid userId,
            ILogger log)
        {
            log.LogInformation(userId.ToString());

            return new OkResult();
        }
    }
}
