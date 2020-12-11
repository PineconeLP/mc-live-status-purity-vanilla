using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.NotificationSupportCheckers
{
    public class NotificationSupportChecker
    {
        private readonly IJSRuntime _js;

        public NotificationSupportChecker(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> IsNotificationSupported()
        {
            return await _js.InvokeAsync<bool>("isNotificationSupported");
        }
    }
}