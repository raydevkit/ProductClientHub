namespace ProductClientHub.App.Navigation;

public interface INavigationService
{
    Task GoToAsync(ShellNavigationState state);
}
