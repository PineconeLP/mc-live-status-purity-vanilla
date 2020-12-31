using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace MCLiveStatus.PurityVanilla.Blazor.Services.Recaptchas
{
    public interface IRecaptchaLoader
    {
        Task LoadRecaptcha(string targetElementId);
        Task LoadRecaptcha(string targetElementId, Action onSubmit);
    }
}