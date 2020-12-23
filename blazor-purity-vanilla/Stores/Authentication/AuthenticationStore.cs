using System;
using System.Threading.Tasks;
using Endpointer.Authentication.Client.Services.Login;
using Endpointer.Authentication.Client.Services.Logout;
using Endpointer.Authentication.Core.Models.Requests;
using Endpointer.Core.Models.Responses;
using MCLiveStatus.PurityVanilla.Blazor.Stores.Tokens;

namespace MCLiveStatus.PurityVanilla.Blazor.Stores.Authentication
{
    public class AuthenticationStore
    {
        private readonly ILoginService _loginService;
        private readonly ILogoutService _logoutService;
        private readonly ITokenStore _tokenStore;

        private TaskCompletionSource<object> _initializeTask;
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

        public async Task Initialize()
        {
            if (_initializeTask == null)
            {
                _initializeTask = new TaskCompletionSource<object>();

                await Load();

                _initializeTask.SetResult(null);
            }

            await _initializeTask.Task;
        }

        private async Task Load()
        {
            if (!_tokenStore.IsAccessTokenExpired || await _tokenStore.HasRefreshToken())
            {
                IsLoggedIn = true;
            }
        }

        public async Task Login(string username, string password)
        {
            LoginRequest request = new LoginRequest()
            {
                Username = username,
                Password = password
            };

            AuthenticatedUserResponse response = await _loginService.Login(request);

            await _tokenStore.SetTokens(response.AccessToken, response.RefreshToken, response.AccessTokenExpirationTime);
            IsLoggedIn = true;
        }

        public async Task Logout()
        {
            try
            {
                await _logoutService.Logout();
            }
            finally
            {
                await _tokenStore.ClearRefreshToken();
                IsLoggedIn = false;
            }
        }

        private void OnIsLoggedInChanged()
        {
            IsLoggedInChanged?.Invoke();
        }
    }
}