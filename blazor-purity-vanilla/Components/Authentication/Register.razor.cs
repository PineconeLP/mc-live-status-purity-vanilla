using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Core.Models.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace MCLiveStatus.PurityVanilla.Blazor.Components.Authentication
{
    public partial class Register : ComponentBase
    {
        [Inject]
        public IRegisterService RegisterService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private EditContext _registerEditContext;

        private RegisterRequest RegisterRequest { get; } = new RegisterRequest();

        private bool CanNotRegister => string.IsNullOrEmpty(RegisterRequest.Email) ||
            string.IsNullOrEmpty(RegisterRequest.Username) ||
            string.IsNullOrEmpty(RegisterRequest.Password) ||
            string.IsNullOrEmpty(RegisterRequest.ConfirmPassword);

        private bool IsRegistering { get; set; }

        protected override void OnInitialized()
        {
            _registerEditContext = new EditContext(RegisterRequest);

            base.OnInitialized();
        }

        private async Task OnRegister()
        {
            if (_registerEditContext.Validate())
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
}