using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;

namespace ProductClientHub.App.UseCases.Products.Delete;

public interface IDeleteProductUseCase
{
    Task Execute(Guid id);
}

public class DeleteProductUseCase(
    IUserApiClient api,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), IDeleteProductUseCase
{
    public async Task Execute(Guid id)
    {
        await ExecuteWithErrorHandlingAsync(async () => await api.DeleteProduct(id));
    }
}
