using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Blazor.Analytics;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public IAnalytics Analytics { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Analytics.TrackNavigation(NavigationManager.Uri);
        }
    }
}