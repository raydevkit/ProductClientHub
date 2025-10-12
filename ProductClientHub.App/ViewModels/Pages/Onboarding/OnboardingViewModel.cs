using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Navigation;

namespace ProductClientHub.App.ViewModels.Pages.Onboarding;

public partial class OnboardingViewModel : ViewModelBase
{

    private readonly INavigationService _navigationService;

    public OnboardingViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    [RelayCommand]
    public async Task LoginWithEmailAndPassword()
    {
        await _navigationService.GoToAsync(RoutePages.LOGIN_PAGE);
    }

    [RelayCommand]

    public async Task GetSignupPage()
    {
        await _navigationService.GoToAsync(RoutePages.SIGNUP_PAGE);
    }
}
