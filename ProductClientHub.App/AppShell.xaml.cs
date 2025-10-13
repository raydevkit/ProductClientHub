using ProductClientHub.App.Views.Pages.Login;
using ProductClientHub.App.Views.Pages.SignUp;
using ProductClientHub.App.Views.Pages.Dashboard;
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
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        }

        // Optional DI-enabled constructor; default constructor still used by App
        public AppShell(ITokenStore tokenStore, IUserApiClient apiClient) : this()
        {
            _tokenStore = tokenStore;
            _apiClient = apiClient;
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            try
            {
                var services = this.Handler?.MauiContext?.Services ?? Application.Current?.Handler?.MauiContext?.Services;
                var api = _apiClient ?? services?.GetService<IUserApiClient>();
                var store = _tokenStore ?? services?.GetService<ITokenStore>();

                if (api != null)
                {
                    try { await api.Logout(); } catch { /* ignore logout errors */ }
                }

                if (store != null)
                {
                    await store.ClearAsync();
                }

                // Navigate to login page
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            catch
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
        }
    }
}
