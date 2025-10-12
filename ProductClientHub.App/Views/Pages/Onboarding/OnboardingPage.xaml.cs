using ProductClientHub.App.ViewModels.Pages.Onboarding;

namespace ProductClientHub.App.Views.Pages.Onboarding;

public partial class OnboardingPage : ContentPage
{
	public OnboardingPage(OnboardingViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

}
