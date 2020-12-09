using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Components.Settings
{
    public partial class NotificationSettings : ComponentBase
    {
        [Parameter]
        public bool HasQueue { get; set; }

        [Parameter]
        public int MaxPlayers { get; set; }

        [Parameter]
        public int MaxPlayersExcludingQueue { get; set; }

        [Parameter]
        public bool AllowNotifyJoinable { get; set; }

        [Parameter]
        public EventCallback<bool> AllowNotifyJoinableChanged { get; set; }

        [Parameter]
        public bool AllowNotifyQueueJoinable { get; set; }

        [Parameter]
        public EventCallback<bool> AllowNotifyQueueJoinableChanged { get; set; }

        private async Task OnAllowNotifyJoinableInput()
        {
            await AllowNotifyJoinableChanged.InvokeAsync(!AllowNotifyJoinable);
        }

        private async Task OnAllowNotifyQueueJoinableInput()
        {
            await AllowNotifyQueueJoinableChanged.InvokeAsync(!AllowNotifyQueueJoinable);
        }
    }
}