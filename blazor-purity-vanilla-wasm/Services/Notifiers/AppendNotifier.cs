using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.Notifiers;

namespace MCLiveStatus.PurityVanilla.Blazor.WASM.Services.Notifiers
{
    public class AppendNotifier : INotifier
    {
        private readonly Append.Blazor.Notifications.INotificationService _notificationService;

        public AppendNotifier(Append.Blazor.Notifications.INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task RequestPermission()
        {
            if (await _notificationService.IsSupportedByBrowserAsync())
            {
                await _notificationService.RequestPermissionAsync();
            }
        }

        public async Task Show(Notification notification)
        {
            if (await _notificationService.IsSupportedByBrowserAsync())
            {
                await _notificationService.CreateAsync(notification.Title, notification.Body);
            }
        }
    }
}