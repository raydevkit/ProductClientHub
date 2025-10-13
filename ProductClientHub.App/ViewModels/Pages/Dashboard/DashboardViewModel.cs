using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Navigation;
using ProductClientHub.App.UseCases.Clients.GetAll;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.ViewModels.Pages.Dashboard;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IGetAllClientsUseCase _getAllClients;
    private readonly INavigationService _nav;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isErrorVisible;

    public ObservableCollection<ResponseShortClientJson> Clients { get; } = new();

    public DashboardViewModel(IGetAllClientsUseCase getAllClients, INavigationService nav)
    {
        _getAllClients = getAllClients;
        _nav = nav;
    }

    [RelayCommand]
    public async Task Load()
    {
        if (IsBusy) return;
        IsBusy = true;
        IsErrorVisible = false;
        ErrorMessage = string.Empty;
        try
        {
            Clients.Clear();
            var items = await _getAllClients.Execute();
            foreach (var c in items)
                Clients.Add(c);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            IsErrorVisible = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
