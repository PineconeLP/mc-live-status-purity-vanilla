using System.Threading.Tasks;
using ElectronNET.API.Entities;
using MCLiveStatus.PurityVanilla.Blazor.Models;
using MCLiveStatus.PurityVanilla.Blazor.Services.Notifiers;

namespace MCLiveStatus.PurityVanilla.Blazor.Desktop.Services.Notifiers
{
    public class ElectronNotifier : INotifier
    {
        public Task Show(Notification notification)
        {
            ElectronNET.API.Electron.Notification.Show(new NotificationOptions(notification.Title, notification.Body));

            return Task.CompletedTask;
        }

        public Task RequestPermission()
        {
            // No permission required.

            return Task.CompletedTask;
        }
    }
}