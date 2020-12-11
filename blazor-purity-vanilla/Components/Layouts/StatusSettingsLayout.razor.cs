using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components.Layouts
{
    public partial class StatusSettingsLayout : ComponentBase
    {
        [Parameter]
        public RenderFragment SettingsContent { get; set; }
    }
}