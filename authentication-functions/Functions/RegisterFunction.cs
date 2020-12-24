using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.Core.Models.Requests;

namespace MCLiveStatus.Authentication.Functions
{
    public class RegisterFunction
    {
        private readonly RegisterEndpointHandler _registerHandler;

        public RegisterFunction(RegisterEndpointHandler registerHandler)
        {
            _registerHandler = registerHandler;
        }

        [FunctionName("RegisterFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")]
            RegisterRequest registrationRequest,
            HttpRequest request,
            ILogger log)
        {
            return await _registerHandler.HandleRegister(registrationRequest);
        }
    }
}
