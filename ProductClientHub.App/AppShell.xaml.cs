using ProductClientHub.App.Views.Pages.Login;
using ProductClientHub.App.Views.Pages.SignUp;
using ProductClientHub.App.Views.Pages.Dashboard;

namespace ProductClientHub.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        }
    }
}
