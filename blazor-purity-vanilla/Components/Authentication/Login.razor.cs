using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Core.Models.Requests;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components.Authentication
{
    public partial class Login : ComponentBase
    {
        [Inject]
        public AuthenticationStore AuthenticationStore { get; set; }

        private LoginRequest LoginRequest { get; } = new LoginRequest();

        private async Task OnLogin()
        {
            try
            {
                await AuthenticationStore.Login(LoginRequest.Username, LoginRequest.Password);
            }
            catch (Exception)
            {

            }
        }
    }
}