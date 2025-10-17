using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Navigation;
using ProductClientHub.App.Services;
using ProductClientHub.App.UseCases.Auth.Register;
using ProductClientHub.App.Validation;

namespace ProductClientHub.App.ViewModels.Pages.SignUp;

public partial class SignUpViewModel(
    INavigationService navigationService,
    IRegisterUserUseCase registerUserUseCase,
    IErrorNotifier notifier) : ObservableObject
{
    private readonly SignUpViewModelValidator _validator = new();

    [ObservableProperty]
    private Models.SignUp model = new();

    
    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    public async Task SignUp()
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

            await registerUserUseCase.Execute(Model);
            await notifier.ShowSuccess("Account created successfully!");
            await navigationService.GoToAsync(RoutePages.LOGIN_PAGE);
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
    private async Task NavigateToLogin()
    {
        await navigationService.GoToAsync(RoutePages.LOGIN_PAGE);
    }
}
