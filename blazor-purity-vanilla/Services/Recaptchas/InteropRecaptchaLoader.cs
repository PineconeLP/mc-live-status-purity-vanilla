using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MCLiveStatus.PurityVanilla.Blazor.Services.Recaptchas
{
    public class InteropRecaptchaLoader : IRecaptchaLoader
    {
        private readonly IJSRuntime _js;
        private readonly string _siteKey;

        public InteropRecaptchaLoader(IJSRuntime js, string siteKey)
        {
            _js = js;
            _siteKey = siteKey;
        }

        public async Task LoadRecaptcha(string targetElementId, Action onSubmit = null, Action onExpire = null)
        {
            DotNetObjectReference<InteropCallbackWrapper> submitWrapper = DotNetObjectReference.Create(new InteropCallbackWrapper(onSubmit));
            DotNetObjectReference<InteropCallbackWrapper> expireWrapper = DotNetObjectReference.Create(new InteropCallbackWrapper(onExpire));
            await _js.InvokeVoidAsync("onloadCallback", targetElementId, _siteKey, submitWrapper, expireWrapper);
        }

        private class InteropCallbackWrapper
        {
            private readonly Action _callback;

            public InteropCallbackWrapper(Action callback)
            {
                _callback = callback;
            }

            [JSInvokable]
            public void Execute()
            {
                _callback?.Invoke();
            }
        }
    }
}