using ProductClientHub.App.Views.Pages.Login;
using ProductClientHub.App.Views.Pages.SignUp;

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
        }
    }
}
