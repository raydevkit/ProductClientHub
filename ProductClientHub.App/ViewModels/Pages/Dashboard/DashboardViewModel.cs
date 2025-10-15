using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductClientHub.App.Services;
using ProductClientHub.App.UseCases.Clients.Create;
using ProductClientHub.App.UseCases.Clients.Delete;
using ProductClientHub.App.UseCases.Clients.GetAll;
using ProductClientHub.App.UseCases.Clients.GetById;
using ProductClientHub.App.UseCases.Clients.Update;
using ProductClientHub.App.UseCases.Products.Create;
using ProductClientHub.App.UseCases.Products.Delete;
using ProductClientHub.App.Validation;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.ViewModels.Pages.Dashboard;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IGetAllClientsUseCase _getAllClients;
    private readonly IGetClientByIdUseCase _getClientById;
    private readonly ICreateClientUseCase _createClient;
    private readonly IUpdateClientUseCase _updateClient;
    private readonly IDeleteClientUseCase _deleteClient;
    private readonly ICreateProductUseCase _createProduct;
    private readonly IDeleteProductUseCase _deleteProduct;
    private readonly IErrorNotifier _notifier;
    private readonly DashboardCreateClientValidator _createClientValidator;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<ClientItemViewModel> Clients { get; } = new();

    public DashboardViewModel(
        IGetAllClientsUseCase getAllClients,
        IGetClientByIdUseCase getClientById,
        ICreateClientUseCase createClient,
        IUpdateClientUseCase updateClient,
        IDeleteClientUseCase deleteClient,
        ICreateProductUseCase createProduct,
        IDeleteProductUseCase deleteProduct,
        IErrorNotifier notifier,
        DashboardCreateClientValidator createClientValidator)
    {
        _getAllClients = getAllClients;
        _getClientById = getClientById;
        _createClient = createClient;
        _updateClient = updateClient;
        _deleteClient = deleteClient;
        _createProduct = createProduct;
        _deleteProduct = deleteProduct;
        _notifier = notifier;
        _createClientValidator = createClientValidator;
    }

    [RelayCommand]
    public async Task Load()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            Clients.Clear();
            var items = await _getAllClients.Execute();

            // Build VMs first, add to collection, then prefetch details in parallel
            var vms = new List<ClientItemViewModel>(items.Count);
            foreach (var c in items)
            {
                var vm = new ClientItemViewModel(c.Id, c.LastName, c.Name, _getClientById, _updateClient, _deleteClient, _createProduct, _deleteProduct, _notifier);
                vms.Add(vm);
                Clients.Add(vm);
            }

            // Prefetch full details so email, codice fiscale and products are ready before tapping
            await Task.WhenAll(vms.Select(vm => vm.PrefetchDetailsAsync()));
        }
        catch (Exception ex)
        {
            await _notifier.ShowError(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Inline create client form state
    [ObservableProperty]
    private bool isCreateClientVisible;

    [ObservableProperty]
    private RequestClientJson newClient = new();

    [RelayCommand]
    private void ToggleCreateClient() => IsCreateClientVisible = !IsCreateClientVisible;

    [RelayCommand]
    private async Task CreateClient()
    {
        // validate
        var validation = await _createClientValidator.ValidateAsync(this);
        if (!validation.IsValid)
        {
            var msg = validation.Errors.First().ErrorMessage;
            await _notifier.ShowError(msg);
            return;
        }

        try
        {
            var created = await _createClient.Execute(NewClient);
            // add to top and reset form
            var vm = new ClientItemViewModel(created.Id, NewClient.LastName, NewClient.Name, _getClientById, _updateClient, _deleteClient, _createProduct, _deleteProduct, _notifier);
            Clients.Insert(0, vm);
            // Prefetch the just-created client's details (if API returns them later)
            _ = vm.PrefetchDetailsAsync();

            NewClient = new();
            IsCreateClientVisible = false;
        }
        catch (Exception ex)
        {
            await _notifier.ShowError(ex.Message);
        }
    }
}

public partial class ClientItemViewModel : ObservableObject
{
    private readonly IGetClientByIdUseCase _getClientById;
    private readonly IUpdateClientUseCase _updateClient;
    private readonly IDeleteClientUseCase _deleteClient;
    private readonly ICreateProductUseCase _createProduct;
    private readonly IDeleteProductUseCase _deleteProduct;
    private readonly IErrorNotifier _notifier;
    private readonly ClientItemEditValidator _editValidator;
    private readonly ClientItemCreateProductValidator _productValidator;

    public Guid Id { get; }

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private bool isExpanded;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string codiceFiscale = string.Empty;

    public ObservableCollection<ResponseShortProductJson> Products { get; } = new();

    // Inline product creation
    [ObservableProperty]
    private bool isCreateProductVisible;

    [ObservableProperty]
    private string newProductName = string.Empty;

    [ObservableProperty]
    private string newProductBrand = string.Empty;

    [ObservableProperty]
    private decimal newProductPrice;

    public ClientItemViewModel(Guid id, string lastName, string name,
        IGetClientByIdUseCase getClientById,
        IUpdateClientUseCase updateClient,
        IDeleteClientUseCase deleteClient,
        ICreateProductUseCase createProduct,
        IDeleteProductUseCase deleteProduct,
        IErrorNotifier notifier)
    {
        Id = id;
        LastName = lastName;
        Name = name;
        _getClientById = getClientById;
        _updateClient = updateClient;
        _deleteClient = deleteClient;
        _createProduct = createProduct;
        _deleteProduct = deleteProduct;
        _notifier = notifier;
        _editValidator = new ClientItemEditValidator();
        _productValidator = new ClientItemCreateProductValidator();
    }

    // Called automatically by MVVM Toolkit when IsExpanded changes
    partial void OnIsExpandedChanged(bool value)
    {
        if (value)
        {
            _ = LoadDetailsAsync();
        }
    }

    // Also ensure toggling edit expands section and loads details if needed
    partial void OnIsEditingChanged(bool value)
    {
        if (value)
        {
            IsExpanded = true;
            if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(CodiceFiscale) && Products.Count == 0)
            {
                _ = LoadDetailsAsync();
            }
        }
    }

    // Public prefetch used by the parent VM
    public Task PrefetchDetailsAsync() => LoadDetailsAsync();

    private async Task LoadDetailsAsync()
    {
        var details = await _getClientById.Execute(Id);
        Email = details.Email;
        CodiceFiscale = details.CodiceFiscale;
        Products.Clear();
        foreach (var p in details.Products)
            Products.Add(p);
    }

    [RelayCommand]
    private void ToggleEdit() => IsEditing = !IsEditing;

    [RelayCommand]
    private async Task SaveEdit()
    {
        var valid = await _editValidator.ValidateAsync(this);
        if (!valid.IsValid)
        {
            var msg = valid.Errors.First().ErrorMessage;
            await _notifier.ShowError(msg);
            return;
        }

        var req = new RequestClientJson { Name = Name, LastName = LastName, Email = Email, CodiceFiscale = CodiceFiscale };
        await _updateClient.Execute(Id, req);
        IsEditing = false;
    }

    [RelayCommand]
    private async Task DeleteClient()
    {
        var windows = Application.Current?.Windows;
        var mainPage = windows is { Count: > 0 } ? windows[0]?.Page : null;
        if (mainPage != null)
        {
            var confirm = await mainPage.DisplayAlertAsync("Confirm", "Delete this client?", "Delete", "Cancel");
            if (!confirm) return;
        }
        await _deleteClient.Execute(Id);
        // let parent refresh list; usually we would raise an event
    }

    [RelayCommand]
    private void ToggleCreateProduct() => IsCreateProductVisible = !IsCreateProductVisible;

    [RelayCommand]
    private async Task CreateProduct()
    {
        var valid = await _productValidator.ValidateAsync(this);
        if (!valid.IsValid)
        {
            var msg = valid.Errors.First().ErrorMessage;
            await _notifier.ShowError(msg);
            return;
        }

        var req = new RequestProductJson { Name = NewProductName, Brand = NewProductBrand, Price = NewProductPrice };
        var created = await _createProduct.Execute(Id, req);
        Products.Add(created);
        NewProductName = string.Empty;
        NewProductBrand = string.Empty;
        NewProductPrice = 0m;
        IsCreateProductVisible = false;
    }

    [RelayCommand]
    private async Task DeleteProduct(Guid productId)
    {
        var windows = Application.Current?.Windows;
        var mainPage = windows is { Count: > 0 } ? windows[0]?.Page : null;
        if (mainPage != null)
        {
            var confirm = await mainPage.DisplayAlertAsync("Confirm", "Delete this product?", "Delete", "Cancel");
            if (!confirm) return;
        }
        await _deleteProduct.Execute(productId);
        var toRemove = Products.FirstOrDefault(p => p.Id == productId);
        if (toRemove != null) Products.Remove(toRemove);
    }
}
