using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Navigation;
using ProductClientHub.App.Services;
using ProductClientHub.App.UseCases.Auth.Login;
using ProductClientHub.App.Validation;
using ProductClientHub.App.Views.Shells;

namespace ProductClientHub.App.ViewModels.Pages.Login;

public partial class LoginViewModel : ObservableObject
{
    private readonly LoginViewModelValidator _validator = new();
    private readonly INavigationService _navigationService;
    private readonly ILoginUseCase _loginUseCase;
    private readonly IErrorNotifier _notifier;

    [ObservableProperty]
    private Models.Login model = new();

    public LoginViewModel(INavigationService navigationService, ILoginUseCase loginUseCase, IErrorNotifier notifier)
    {
        _navigationService = navigationService;
        _loginUseCase = loginUseCase;
        _notifier = notifier;
    }

    [RelayCommand]
    private async Task Login()
    {
        // Validate using FluentValidation
        var validationResult = await _validator.ValidateAsync(this);
        if (!validationResult.IsValid)
        {
            await _notifier.ShowError(validationResult.Errors.First().ErrorMessage);
            return;
        }

        try
        {
            await _loginUseCase.Execute(Model.Email, Model.Password);

            // Swap to authenticated shell using window root page (no obsolete API)
            var window = Application.Current!.Windows.First();
            window.Page = new DashboardShell();
        }
        catch (Exception ex)
        {
            await _notifier.ShowError($"Login failed: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task NavigateToSignUp()
    {
        await _navigationService.GoToAsync(RoutePages.SIGNUP_PAGE);
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        var windows = Application.Current?.Windows;
        if (windows is { Count: > 0 })
        {
            var mainPage = windows[0]?.Page;
            if (mainPage != null)
            {
                await mainPage.DisplayAlertAsync("Info", "Forgot password feature coming soon!", "OK");
            }
        }
    }
}
