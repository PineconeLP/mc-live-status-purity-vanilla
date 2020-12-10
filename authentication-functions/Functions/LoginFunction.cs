using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BreadAuthentication.Core.Models.Requests;
using BreadAuthentication.Core.EndpointHandlers;

namespace MCLiveStatus.Authentication.Functions
{
    public class LoginFunction
    {
        private readonly LoginEndpointHandler _loginHandler;

        public LoginFunction(LoginEndpointHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        [FunctionName("LoginFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")]
            LoginRequest loginRequest,
            HttpRequest request,
            ILogger log)
        {
            return await _loginHandler.HandleLogin(loginRequest);
        }
    }
}
