using System;
using System.Threading.Tasks;

namespace MCLiveStatus.PurityVanilla.Blazor.Services.Recaptchas
{
    public interface IRecaptchaLoader
    {
        Task LoadRecaptcha(string targetElementId, Action onSubmit = null, Action onExpire = null);
    }
}