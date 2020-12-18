using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Core.Models.Requests;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace MCLiveStatus.PurityVanilla.Blazor.Components.Authentication
{
    public partial class Login : ComponentBase
    {
        [Inject]
        public AuthenticationStore AuthenticationStore { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private EditContext _loginEditContext;

        private LoginRequest LoginRequest { get; } = new LoginRequest();

        private bool CanNotLogin => string.IsNullOrEmpty(LoginRequest.Username) || string.IsNullOrEmpty(LoginRequest.Password);

        private bool IsLoggingIn { get; set; }

        protected override void OnInitialized()
        {
            _loginEditContext = new EditContext(LoginRequest);

            base.OnInitialized();
        }

        private async Task OnLogin()
        {
            if (_loginEditContext.Validate())
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
}