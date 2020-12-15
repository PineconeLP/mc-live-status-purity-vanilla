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

        protected override void OnInitialized()
        {
            AuthenticationStore.IsLoggedInChanged += StateHasChanged;

            base.OnInitialized();
        }

        private async Task Logout()
        {
            await AuthenticationStore.Logout();
        }

        public void Dispose()
        {
            AuthenticationStore.IsLoggedInChanged -= StateHasChanged;
        }
    }
}