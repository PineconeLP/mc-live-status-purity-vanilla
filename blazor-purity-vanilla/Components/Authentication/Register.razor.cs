using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Core.Models.Requests;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components.Authentication
{
    public partial class Register : ComponentBase
    {
        [Inject]
        public IRegisterService RegisterService { get; set; }

        private RegisterRequest RegisterRequest { get; } = new RegisterRequest();

        private async Task OnRegister()
        {
            try
            {
                await RegisterService.Register(RegisterRequest);
            }
            catch (Exception)
            {

            }
        }
    }
}