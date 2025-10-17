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

public partial class DashboardViewModel(
    IGetAllClientsUseCase getAllClients,
    IGetClientByIdUseCase getClientById,
    ICreateClientUseCase createClient,
    IUpdateClientUseCase updateClient,
    IDeleteClientUseCase deleteClient,
    ICreateProductUseCase createProduct,
    IDeleteProductUseCase deleteProduct,
    IErrorNotifier notifier,
    DashboardCreateClientValidator createClientValidator) : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<ClientItemViewModel> Clients { get; } = [];

    [RelayCommand]
    public async Task Load()
    {
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            Clients.Clear();
            var items = await getAllClients.Execute();

            
            var vms = new List<ClientItemViewModel>();
            foreach (var c in items)
            {
                var vm = new ClientItemViewModel(c.Id, c.LastName, c.Name, getClientById, updateClient, deleteClient, createProduct, deleteProduct, notifier);
                Clients.Add(vm);
                vms.Add(vm);
            }

         
            await Task.WhenAll(vms.Select(vm => vm.PrefetchDetailsAsync()));
        }
        catch
        {
            // Error already handled and displayed by UseCase
        }
        finally
        {
            IsBusy = false;
        }
    }

    
    [ObservableProperty]
    private bool isCreateClientVisible;

    [ObservableProperty]
    private RequestClientJson newClient = new();

    [RelayCommand]
    private void ToggleCreateClient() => IsCreateClientVisible = !IsCreateClientVisible;

    [RelayCommand]
    private async Task CreateClient()
    {
        
        var validation = await createClientValidator.ValidateAsync(this);
        if (!validation.IsValid)
        {
            var msg = validation.Errors[0].ErrorMessage;
            await notifier.ShowError(msg);
            return;
        }

        try
        {
            var created = await createClient.Execute(NewClient);
            var vm = new ClientItemViewModel(created.Id, NewClient.LastName, NewClient.Name, getClientById, updateClient, deleteClient, createProduct, deleteProduct, notifier);
            Clients.Insert(0, vm);
            _ = vm.PrefetchDetailsAsync();

            NewClient = new();
            IsCreateClientVisible = false;
            
            await notifier.ShowSuccess("Client created successfully!");
        }
        catch
        {
           
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

    public ObservableCollection<ResponseShortProductJson> Products { get; } = [];

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

    partial void OnIsExpandedChanged(bool value)
    {
        if (value)
        {
            _ = LoadDetailsAsync();
        }
    }

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

    public Task PrefetchDetailsAsync() => LoadDetailsAsync();

    private async Task LoadDetailsAsync()
    {
        try
        {
            var details = await _getClientById.Execute(Id);
            Email = details.Email;
            CodiceFiscale = details.CodiceFiscale;
            Products.Clear();
            foreach (var p in details.Products)
                Products.Add(p);
        }
        catch
        {
            
        }
    }

    [RelayCommand]
    private void ToggleEdit() => IsEditing = !IsEditing;

    [RelayCommand]
    private async Task SaveEdit()
    {
        var valid = await _editValidator.ValidateAsync(this);
        if (!valid.IsValid)
        {
            var msg = valid.Errors[0].ErrorMessage;
            await _notifier.ShowError(msg);
            return;
        }

        try
        {
            var req = new RequestClientJson { Name = Name, LastName = LastName, Email = Email, CodiceFiscale = CodiceFiscale };
            await _updateClient.Execute(Id, req);
            IsEditing = false;
            await _notifier.ShowSuccess("Client updated successfully!");
        }
        catch
        {
            
        }
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
        
        try
        {
            await _deleteClient.Execute(Id);
            await _notifier.ShowSuccess("Client deleted successfully!");
        }
        catch
        {
            
        }
    }

    [RelayCommand]
    private void ToggleCreateProduct() => IsCreateProductVisible = !IsCreateProductVisible;

    [RelayCommand]
    private async Task CreateProduct()
    {
        var valid = await _productValidator.ValidateAsync(this);
        if (!valid.IsValid)
        {
            var msg = valid.Errors[0].ErrorMessage;
            await _notifier.ShowError(msg);
            return;
        }

        try
        {
            var req = new RequestProductJson { Name = NewProductName, Brand = NewProductBrand, Price = NewProductPrice };
            var created = await _createProduct.Execute(Id, req);
            Products.Add(created);
            NewProductName = string.Empty;
            NewProductBrand = string.Empty;
            NewProductPrice = 0m;
            IsCreateProductVisible = false;
            await _notifier.ShowSuccess("Product created successfully!");
        }
        catch
        {
            
        }
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
        
        try
        {
            await _deleteProduct.Execute(productId);
            var toRemove = Products.FirstOrDefault(p => p.Id == productId);
            if (toRemove != null) Products.Remove(toRemove);
            await _notifier.ShowSuccess("Product deleted successfully!");
        }
        catch
        {
            
        }
    }
}
