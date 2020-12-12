using MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject]
        public AuthenticationStore AuthenticationStore { get; set; }
    }
}