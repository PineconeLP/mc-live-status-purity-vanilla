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

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private RegisterRequest RegisterRequest { get; } = new RegisterRequest();

        private bool IsRegistering { get; set; }

        private async Task OnRegister()
        {
            IsRegistering = true;
            StateHasChanged();

            try
            {
                await RegisterService.Register(RegisterRequest);
                NavigationManager.NavigateTo("/login");
            }
            catch (Exception)
            {
            }
            finally
            {
                IsRegistering = false;
                StateHasChanged();
            }
        }
    }
}