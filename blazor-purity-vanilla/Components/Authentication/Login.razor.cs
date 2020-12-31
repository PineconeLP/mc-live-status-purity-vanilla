using System;
using System.IO;
using System.Threading.Tasks;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Client.Exceptions;
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

        private bool CanNotLogin => IsLoggingIn || string.IsNullOrEmpty(LoginRequest.Username) || string.IsNullOrEmpty(LoginRequest.Password);

        private bool IsLoggingIn { get; set; }
        private bool HasInvalidCredentials { get; set; }
        private bool LoginFailed { get; set; }

        private string RegisterRedirect => Path.Combine(NavigationManager.BaseUri, "register");

        protected override void OnInitialized()
        {
            _loginEditContext = new EditContext(LoginRequest);

            base.OnInitialized();
        }

        private async Task OnLogin()
        {
            ClearErrors();

            if (_loginEditContext.Validate())
            {
                IsLoggingIn = true;
                StateHasChanged();

                try
                {
                    await AuthenticationStore.Login(LoginRequest.Username, LoginRequest.Password);
                    NavigationManager.NavigateTo(NavigationManager.BaseUri);
                }
                catch (UnauthorizedException)
                {
                    HasInvalidCredentials = true;
                }
                catch (Exception)
                {
                    LoginFailed = true;
                }

                IsLoggingIn = false;
                StateHasChanged();
            }
        }

        private void ClearErrors()
        {
            HasInvalidCredentials = false;
            LoginFailed = false;
        }
    }
}