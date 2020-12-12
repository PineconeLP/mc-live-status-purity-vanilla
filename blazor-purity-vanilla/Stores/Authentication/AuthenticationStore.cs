using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Client.Services;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Authentication.Core.Models.Responses;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication
{
    public class AuthenticationStore
    {
        private readonly ILoginService _loginService;
        private readonly ILogoutService _logoutService;
        private readonly ITokenStore _tokenStore;

        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            private set
            {
                _isLoggedIn = value;
                OnIsLoggedInChanged();
            }
        }

        public event Action IsLoggedInChanged;

        public AuthenticationStore(ILoginService loginService, ILogoutService logoutService, ITokenStore tokenStore)
        {
            _loginService = loginService;
            _logoutService = logoutService;
            _tokenStore = tokenStore;
        }

        public async Task Login(string username, string password)
        {
            LoginRequest request = new LoginRequest()
            {
                Username = username,
                Password = password
            };

            try
            {
                SuccessResponse<AuthenticatedUserResponse> response = await _loginService.Login(request);
                AuthenticatedUserResponse responseData = response.Data;

                if (responseData != null)
                {
                    _tokenStore.AccessToken = responseData.AccessToken;
                    await _tokenStore.SetRefreshToken(responseData.RefreshToken);
                    IsLoggedIn = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Logout()
        {
            try
            {
                await _logoutService.Logout(_tokenStore.AccessToken);
                IsLoggedIn = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OnIsLoggedInChanged()
        {
            IsLoggedInChanged?.Invoke();
        }
    }
}