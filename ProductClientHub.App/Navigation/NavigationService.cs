
namespace ProductClientHub.App.Navigation;

internal class NavigationService : INavigationService
{
    public async Task GoToAsync(ShellNavigationState state)
    {
        await Shell.Current.GoToAsync(state);
    }
}
