using ProductClientHub.App.Data.Network.Api;
using ProductClientHub.App.Services;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.App.UseCases.Products.Create;

public interface ICreateProductUseCase
{
    Task<ResponseShortProductJson> Execute(Guid clientId, RequestProductJson request);
}

public class CreateProductUseCase(
    IUserApiClient api,
    IApiErrorHandler errorHandler) : BaseUseCase(errorHandler), ICreateProductUseCase
{
    public async Task<ResponseShortProductJson> Execute(Guid clientId, RequestProductJson request)
    {
        return await ExecuteWithErrorHandlingAsync(async () => await api.RegisterProduct(clientId, request));
    }
}
