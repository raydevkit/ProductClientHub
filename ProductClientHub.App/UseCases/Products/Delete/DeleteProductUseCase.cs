using ProductClientHub.App.Data.Network.Api;

namespace ProductClientHub.App.UseCases.Products.Delete;

public class DeleteProductUseCase : IDeleteProductUseCase
{
    private readonly IUserApiClient _api;

    public DeleteProductUseCase(IUserApiClient api)
    {
        _api = api;
    }

    public async Task Execute(Guid id)
    {
        await _api.DeleteProduct(id);
    }
}
