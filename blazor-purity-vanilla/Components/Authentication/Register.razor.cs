using System;
using System.IO;
using System.Threading.Tasks;
using Endpointer.Authentication.Client.Exceptions;
using Endpointer.Authentication.Client.Services.Register;
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

        private bool CanNotRegister => IsRegistering ||
            string.IsNullOrEmpty(RegisterRequest.Email) ||
            string.IsNullOrEmpty(RegisterRequest.Username) ||
            string.IsNullOrEmpty(RegisterRequest.Password) ||
            string.IsNullOrEmpty(RegisterRequest.ConfirmPassword);

        private bool IsRegistering { get; set; }

        private bool PasswordsDoNotMatch { get; set; }
        private bool EmailAlreadyExists { get; set; }
        private string AlreadyExistingEmail { get; set; }
        private bool UsernameAlreadyExists { get; set; }
        private string AlreadyExistingUsername { get; set; }
        private bool RegisterFailed { get; set; }

        protected override void OnInitialized()
        {
            _registerEditContext = new EditContext(RegisterRequest);

            base.OnInitialized();
        }

        private async Task OnRegister()
        {
            ClearErrors();

            if (_registerEditContext.Validate())
            {
                if (RegisterRequest.Password != RegisterRequest.ConfirmPassword)
                {
                    PasswordsDoNotMatch = true;
                }
                else
                {
                    IsRegistering = true;
                    StateHasChanged();

                    try
                    {
                        await RegisterService.Register(RegisterRequest);
                        NavigationManager.NavigateTo(Path.Combine(NavigationManager.BaseUri, "login"));
                    }
                    catch (ConfirmPasswordException)
                    {
                        PasswordsDoNotMatch = true;
                    }
                    catch (EmailAlreadyExistsException)
                    {
                        EmailAlreadyExists = true;
                        AlreadyExistingEmail = RegisterRequest.Email;
                    }
                    catch (UsernameAlreadyExistsException)
                    {
                        UsernameAlreadyExists = true;
                        AlreadyExistingUsername = RegisterRequest.Username;
                    }
                    catch (Exception)
                    {
                        RegisterFailed = true;
                    }

                    IsRegistering = false;
                    StateHasChanged();
                }
            }
        }

        private void ClearErrors()
        {
            PasswordsDoNotMatch = false;
            EmailAlreadyExists = false;
            AlreadyExistingEmail = string.Empty;
            UsernameAlreadyExists = false;
            AlreadyExistingUsername = string.Empty;
            RegisterFailed = false;
        }
    }
}