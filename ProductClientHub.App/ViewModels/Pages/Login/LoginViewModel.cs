using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Navigation;
using ProductClientHub.App.Validation;

namespace ProductClientHub.App.ViewModels.Pages.Login;

public partial class LoginViewModel : ObservableObject
{
    private readonly LoginViewModelValidator _validator = new();
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private Models.Login model = new();

    public LoginViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task Login()
    {
        // Hide previous error
        Model.IsErrorVisible = false;
        Model.ErrorMessage = string.Empty;

        // Validate using FluentValidation
        var validationResult = await _validator.ValidateAsync(this);
        if (!validationResult.IsValid)
        {
            ShowError(validationResult.Errors.First().ErrorMessage);
            return;
        }

        try
        {
            // TODO: Call API to login user
            // var request = new RequestLoginJson
            // {
            //     Email = Model.Email,
            //     Password = Model.Password
            // };
            
            // Simulate API call
            await Task.Delay(500);

            var windows = Application.Current?.Windows;
            if (windows is { Count: > 0 })
            {
                var mainPage = windows[0]?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlertAsync("Success", "Login successful!", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            ShowError($"Login failed: {ex.Message}");
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

    private void ShowError(string message)
    {
        Model.ErrorMessage = message;
        Model.IsErrorVisible = true;
    }
}
