using ProductClientHub.App.Views.Pages.Login;
using ProductClientHub.App.Views.Pages.SignUp;
using ProductClientHub.App.Data.Auth;
using ProductClientHub.App.Data.Network.Api;
using Microsoft.Extensions.DependencyInjection;

namespace ProductClientHub.App
{
    public partial class AppShell : Shell
    {
        private readonly ITokenStore? _tokenStore;
        private readonly IUserApiClient? _apiClient;

        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            // Dashboard is in a separate shell, so we don't register a global route here
        }

        public AppShell(ITokenStore tokenStore, IUserApiClient apiClient) : this()
        {
            _tokenStore = tokenStore;
            _apiClient = apiClient;
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Not used in onboarding-only shell; show async alert to avoid obsolete API warning
            var windows = Application.Current?.Windows;
            var mainPage = windows is { Count: > 0 } ? windows[0]?.Page : null;
            if (mainPage != null)
            {
                await mainPage.DisplayAlertAsync("Info", "Logout not available here.", "OK");
            }
        }
    }
}
