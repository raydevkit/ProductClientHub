using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Navigation;
using ProductClientHub.App.Services;
using ProductClientHub.App.UseCases.Auth.Register;
using ProductClientHub.App.Validation;

namespace ProductClientHub.App.ViewModels.Pages.SignUp;

public partial class SignUpViewModel : ObservableObject
{
    private readonly SignUpViewModelValidator _validator = new();
    private readonly INavigationService _navigationService;
    private readonly IRegisterUserUseCase _registerUserUseCase;
    private readonly IErrorNotifier _notifier;

    [ObservableProperty]
    private Models.SignUp model = new();

    public SignUpViewModel(INavigationService navigationService, IRegisterUserUseCase registerUserUseCase, IErrorNotifier notifier)
    {
        _navigationService = navigationService;
        _registerUserUseCase = registerUserUseCase;
        _notifier = notifier;
    }

    [RelayCommand]
    public async Task SignUp()
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
            await _registerUserUseCase.Execute(Model);
            var windows = Application.Current?.Windows;
            if (windows is { Count: > 0 })
            {
                var mainPage = windows[0]?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlertAsync("Success", "Account created successfully!", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await _notifier.ShowError($"Sign up failed: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task NavigateToLogin()
    {
        await _navigationService.GoToAsync(RoutePages.LOGIN_PAGE);
    }
}
