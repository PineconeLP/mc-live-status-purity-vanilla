using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Endpointer.Authentication.API.EndpointHandlers;

namespace MCLiveStatus.Authentication.Functions
{
    public class LogoutFunction
    {
        private readonly LogoutEndpointHandler _logoutHandler;

        public LogoutFunction(LogoutEndpointHandler logoutHandler)
        {
            _logoutHandler = logoutHandler;
        }

        [FunctionName("LogoutFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "logout")]
            HttpRequest request,
            ILogger log)
        {
            return await _logoutHandler.HandleLogout(request);
        }
    }
}
