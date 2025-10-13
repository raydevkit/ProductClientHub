using ProductClientHub.App.ViewModels.Pages.Dashboard;

namespace ProductClientHub.App.Views.Pages.Dashboard;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DashboardViewModel vm)
        {
            await vm.Load();
        }
    }
}
