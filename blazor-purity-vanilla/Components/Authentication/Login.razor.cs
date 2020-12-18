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

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private LoginRequest LoginRequest { get; } = new LoginRequest();

        private bool IsLoggingIn { get; set; }

        private async Task OnLogin()
        {
            IsLoggingIn = true;
            StateHasChanged();

            try
            {
                await AuthenticationStore.Login(LoginRequest.Username, LoginRequest.Password);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception)
            {

            }
            finally
            {
                IsLoggingIn = false;
                StateHasChanged();
            }
        }
    }
}