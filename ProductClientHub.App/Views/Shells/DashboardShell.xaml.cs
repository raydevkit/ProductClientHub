using ProductClientHub.App.Data.Auth;
using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Views.Pages.Login;
using Microsoft.Extensions.DependencyInjection;

namespace ProductClientHub.App.Views.Shells;

public partial class DashboardShell : Shell
{
    private readonly ITokenStore? _tokenStore;
    private readonly IUserApiClient? _apiClient;

    public DashboardShell()
    {
        InitializeComponent();
    }

    public DashboardShell(ITokenStore tokenStore, IUserApiClient apiClient) : this()
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
                try { await api.Logout(); } catch { }
            }
            if (store != null)
            {
                await store.ClearAsync();
            }

            var window = Application.Current!.Windows.First();
            window.Page = new AppShell();
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        catch
        {
            var window = Application.Current!.Windows.First();
            window.Page = new AppShell();
        }
    }
}
