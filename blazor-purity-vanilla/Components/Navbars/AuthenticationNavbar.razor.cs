using System;
using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components.Navbars
{
    public partial class AuthenticationNavbar : ComponentBase, IDisposable
    {
        [Inject]
        public AuthenticationStore AuthenticationStore { get; set; }

        private bool IsLoggedIn => AuthenticationStore.IsLoggedIn;
        private bool IsLoggingOut { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationStore.Initialize();
            AuthenticationStore.IsLoggedInChanged += StateHasChanged;

            await base.OnInitializedAsync();
        }

        private async Task Logout()
        {
            IsLoggingOut = true;
            StateHasChanged();

            try
            {
                await AuthenticationStore.Logout();
            }
            catch (Exception)
            {
                // Just do nothing for now. 
            }

            IsLoggingOut = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            AuthenticationStore.IsLoggedInChanged -= StateHasChanged;
        }
    }
}