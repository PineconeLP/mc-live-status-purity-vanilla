using System.Threading.Tasks;
using MCLiveStatus.PurityVanilla.Blazor.Models;

namespace MCLiveStatus.PurityVanilla.Blazor.Services.Notifiers
{
    public interface INotifier
    {
        Task Show(Notification notification);
        Task RequestPermission();
    }
}