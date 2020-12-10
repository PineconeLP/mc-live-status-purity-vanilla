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

namespace MCLiveStatus.Authentication.Functions
{
    public class RegisterFunction
    {
        private readonly Authenticator _authenticator;

        public RegisterFunction(Authenticator authenticator)
        {
            _authenticator = authenticator;
        }

        [FunctionName("RegisterFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")]
            RegistrationRequest registrationRequest,
            HttpRequest request,
            ILogger log)
        {
            try
            {
                await _authenticator.Register(
                    registrationRequest.Email,
                    registrationRequest.Username,
                    registrationRequest.Password,
                    registrationRequest.ConfirmPassword
                );

                return new OkResult();
            }
            catch (PasswordsDoNotMatchException)
            {
                return new BadRequestResult();
            }
            catch (EmailExistsException)
            {
                return new ConflictResult();
            }
            catch (UsernameExistsException)
            {
                return new ConflictResult();
            }
        }
    }
}
