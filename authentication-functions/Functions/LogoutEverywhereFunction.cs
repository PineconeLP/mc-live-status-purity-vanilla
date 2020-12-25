using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Endpointer.Authentication.API.EndpointHandlers;

namespace MCLiveStatus.Authentication.Functions
{
    public class LogoutEverywhereFunction
    {
        private readonly LogoutEverywhereEndpointHandler _logoutEverywhereHandler;

        public LogoutEverywhereFunction(LogoutEverywhereEndpointHandler logoutEverywhereHandler)
        {
            _logoutEverywhereHandler = logoutEverywhereHandler;
        }

        [FunctionName("LogoutEverywhereFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "logout")]
            HttpRequest request,
            ILogger log)
        {
            return await _logoutEverywhereHandler.HandleLogoutEverywhere(request);
        }
    }
}
