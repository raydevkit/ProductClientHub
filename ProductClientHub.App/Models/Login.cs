using CommunityToolkit.Mvvm.ComponentModel;

namespace ProductClientHub.App.Models;

public partial class Login : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isErrorVisible;
}
