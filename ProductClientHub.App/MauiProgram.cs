using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductClientHub.App.Data.Auth;
using ProductClientHub.App.Data.Network;
using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Navigation;
using ProductClientHub.App.UseCases.Auth.Login;
using ProductClientHub.App.UseCases.Auth.Register;
using ProductClientHub.App.UseCases.Clients.GetAll;
using ProductClientHub.App.ViewModels.Pages.Dashboard;
using ProductClientHub.App.ViewModels.Pages.Login;
using ProductClientHub.App.ViewModels.Pages.Onboarding;
using ProductClientHub.App.ViewModels.Pages.SignUp;
using ProductClientHub.App.Views.Pages.Dashboard;
using ProductClientHub.App.Views.Pages.Login;
using ProductClientHub.App.Views.Pages.SignUp;
using Refit;
using System.Reflection;


namespace ProductClientHub.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .AddPages()
                .AddNavigationService()
                .AddAppSettings()
                .AddHttpClients()
                .AddUseCases()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter_24pt-Medium.ttf", "InterMedium");
                fonts.AddFont("Inter_24pt-Light.ttf", "InterLight");
                fonts.AddFont("Inter_24pt-Bold.ttf", "InterBold");
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }

        private static MauiAppBuilder AddPages(this MauiAppBuilder appBuilder)
        {
            appBuilder.Services.AddTransient<OnboardingViewModel>();
            appBuilder.Services.AddScopedWithShellRoute<LoginPage, LoginViewModel>(RoutePages.LOGIN_PAGE);
            appBuilder.Services.AddScopedWithShellRoute<SignUpPage, SignUpViewModel>(RoutePages.SIGNUP_PAGE);
            appBuilder.Services.AddScopedWithShellRoute<DashboardPage, DashboardViewModel>(RoutePages.DASHBOARD_PAGE);
            return appBuilder;
        }

        private static MauiAppBuilder AddNavigationService(this MauiAppBuilder appBuilder)
        {
            appBuilder.Services.AddSingleton<INavigationService, NavigationService>();
            return appBuilder;
        }

        private static MauiAppBuilder AddAppSettings(this MauiAppBuilder appBuilder)
        {
            using var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ProductClientHub.App.appsettings.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(fileStream!)
                .Build();

            appBuilder.Configuration.AddConfiguration(config);

            return appBuilder;
        }

        private static MauiAppBuilder AddHttpClients(this MauiAppBuilder appBuilder)

        {
            var apiUrl = appBuilder.Configuration.GetValue<string>("ApiUrl")!;

            // token store + auth handler
            appBuilder.Services.AddSingleton<ITokenStore, SecureTokenStore>();
            appBuilder.Services.AddTransient<AuthHeaderHandler>();

            appBuilder.Services.AddRefitClient<IUserApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrl))
                .AddHttpMessageHandler<AuthHeaderHandler>();

            return appBuilder;

        }

        private static MauiAppBuilder AddUseCases(this MauiAppBuilder appBuilder)
        {
            appBuilder.Services.AddTransient<IRegisterUserUseCase, RegisterUserUseCase>();
            appBuilder.Services.AddTransient<ILoginUseCase, LoginUseCase>();
            appBuilder.Services.AddTransient<IGetAllClientsUseCase, GetAllClientsUseCase>();
            return appBuilder;
        }
    }
}