using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Navigation;
using ProductClientHub.App.Services;
using ProductClientHub.App.UseCases.Auth.Login;
using ProductClientHub.App.Validation;
using ProductClientHub.App.Views.Shells;

namespace ProductClientHub.App.ViewModels.Pages.Login;

public partial class LoginViewModel(
    INavigationService navigationService,
    ILoginUseCase loginUseCase,
    IErrorNotifier notifier) : ObservableObject
{
    private readonly LoginViewModelValidator _validator = new();

    [ObservableProperty]
    private Models.Login model = new();

    // Busy indicator like Dashboard
    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task Login()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
           
            var validationResult = await _validator.ValidateAsync(this);
            if (!validationResult.IsValid)
            {
                await notifier.ShowError(validationResult.Errors.First().ErrorMessage);
                return;
            }

            await loginUseCase.Execute(Model.Email, Model.Password);

            
            var window = Application.Current!.Windows[0];
            window.Page = new DashboardShell();
        }
        catch
        {
           
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToSignUp()
    {
        await navigationService.GoToAsync(RoutePages.SIGNUP_PAGE);
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        await notifier.ShowError("Forgot password feature coming soon!");
    }
}
