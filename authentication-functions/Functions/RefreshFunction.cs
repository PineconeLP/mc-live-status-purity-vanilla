using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Endpointer.Authentication.API.EndpointHandlers;
using Endpointer.Authentication.API.Models.Requests;

namespace MCLiveStatus.Authentication.Functions
{
    public class RefreshFunction
    {
        private readonly RefreshEndpointHandler _refreshHandler;

        public RefreshFunction(RefreshEndpointHandler refreshHandler)
        {
            _refreshHandler = refreshHandler;
        }

        [FunctionName("RefreshFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "refresh")]
            RefreshRequest refreshTokenRequest,
            HttpRequest request,
            ILogger log)
        {
            return await _refreshHandler.HandleRefresh(refreshTokenRequest);
        }
    }
}
